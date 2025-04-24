import jupyterhub
from jupyterhub.spawner import Spawner
from jupyterhub.utils import exponential_backoff, maybe_future
from .utils import JupyternetesUtils, get_default_template_namespace
from .clients import JupyterNotebookInstanceClient
from jupyterhub.traitlets import Unicode, Integer
from .models import V1JupyterNotebookInstance
from ._version import __version__
from os import environ, path
from kubernetes_asyncio import config



class JupyternetesSpawner(Spawner):
    utils : JupyternetesUtils = None
    instance_client : JupyterNotebookInstanceClient = None

    template_name = Unicode(
        default_value=environ.get("JUPYTERNETES_TEMPLATE", "default-template"),
        help = """
        The name of the template to use for this instance
        """
    ).tag(config=True)

    template_namespace = Unicode(
        default_value = get_default_template_namespace(),
        help = """
        The namespace of the template to use for this instance
        """
    ).tag(config=True)

    instance_name = Unicode(
        help = """
        The name of the instance being created
        """
    ).tag(config=True)

    instance_namespace = Unicode(
        default_value="default",
        help = """
        The name of the instance being created
        """
    ).tag(config=True)
    

    instance_protocol = Unicode(
        default_value="http",
        help = """
        The protocol to use for the instance
        """
    ).tag(config=True)

    instance_port = Integer(
        default_value=8888,
        help = """
        The default instance port
        """
    ).tag(config=True)

    
    status_check_max_wait = Integer(
        default_value=5,
        help = """
        Max wait in seconds for each status check
        """
    ).tag(config=True)


    
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.instance_client = None
        try:
            self.log.debug(f"Jupyternetes Spawner Version {__version__} initializing")
            self.utils = JupyternetesUtils(self)
            self.log.debug("Jupyternetes Spawner initialized")
        except Exception as e:
            self.log.error(f"Error initializing Jupyternetes Spawner: {e}")
            raise e

    def load_state(self, state):
        """
        override from inherited class:

        Load the state of the spawner from the given state dictionary.
        """
        try:
            self.log.debug("Jupyternetes Spawner Loading State")
            super().load_state(state)
            #self.instance_name = state.get("instance_name", self.instance_name)
            #self.instance_namespace = state.get("instance_namespace", self.instance_namespace)
            self.log.debug("Jupyternetes Spawner Loaded State")

        except Exception as e:
            self.log.error(f"Error loading state: {e}")
            raise e

    def get_state(self):
        """
        override from inherited class:

        Get the state of the spawner as a dictionary.
        """
        try:
            self.log.debug("Jupyternetes Getting Spawner State")
            state = super().get_state()
            #if self.instance_name is not None:
            #    state["instance_name"] = self.instance_name
            #
            #if self.instance_namespace is not None:
            #    state["instance_namespace"] = self.instance_namespace
            
            self.log.debug("Jupyternetes Got Spawner State")
            return state
        except Exception as e:
            self.log.error(f"Error getting state: {e}")
            raise e
        
    async def init_client(self):
        if self.instance_client is None:
            self.log.debug("Initializing JupyterNotebookInstanceClient")

            self.log.debug(f"Configuration is not provided, loading kubernetes config")
            
            configuration = None

            kubernetes_service_host = environ.get("KUBERNETES_SERVICE_HOST")

            if kubernetes_service_host:
                self.log.debug(f"Kubernetes service host found: {kubernetes_service_host}, implying this is running in cluster")
                configuration = config.load_incluster_config()
            else:
                self.log.debug(f"Kubernetes service host not found, attempting to load kube config from local folders")
                configuration = await config.load_kube_config() 

            self.instance_client = JupyterNotebookInstanceClient(configuration=configuration)
            

    async def start(self) -> str:
        """
        override from inherited class:

        Start the spawner.
        """
        try:
            self.log.debug("Starting Jupyternetes Spawner")
            await self.init_client()
            pod_address, port_number = await self.utils.start_instance()
            instance_address = f"{self.instance_protocol}://{pod_address}:{port_number}"
            self.log.debug(f"instance {self.instance_name} on {self.instance_namespace} is at url: \"{instance_address}\"")
            return instance_address
        
        except Exception as e:
            self.log.error(f"Error starting instance: {e}")
            raise e

    async def stop(self, now=False):
        """
        override from inherited class:

        Stop the spawner.
        """
        try:
            self.log.info("Stopping Jupyternetes Spawner")
            if not now:
                await self.init_client()
                self.log.info("Gracefully stopping instances")
                instance_list = await self.instance_client.list(self.instance_namespace, field_selector=f"metadata.name={self.instance_name}")
                if len(instance_list.items) > 0:
                    self.log.info(f"Deleting instance: {self.instance_name} on namespace: {self.instance_namespace}")
                    await self.instance_client.delete(namespace = self.instance_namespace, name = self.instance_name)
                    self.log.info("Instance deleted")
            self.log.info("Stopped Jupyternetes Spawner")
        except Exception as e:
            self.log.error(f"Error stopping instance: {e}")
            raise e

    async def poll(self):
        """
        override from inherited class:

        Poll the spawner.
        """
        try:
            self.log.debug(f"Polling Spawner for {self.instance_name} in {self.instance_namespace}")
            await self.init_client()
            instance_list = await self.instance_client.list(self.instance_namespace, field_selector=f"metadata.name={self.instance_name}")
            if len(instance_list.items) > 0:
                self.log.debug(f"Polling Returning None for {self.instance_name} in {self.instance_namespace}")
                return None
            
            self.log.debug(f"Polling Returning 0 for {self.instance_name} in {self.instance_namespace}")
            return 0
        except Exception as e:
            self.log.error(f"Error polling instance: {e}")
            raise e