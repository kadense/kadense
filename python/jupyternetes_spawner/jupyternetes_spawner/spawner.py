import jupyterhub
from jupyterhub.spawner import Spawner
from jupyterhub.utils import exponential_backoff, maybe_future
from .utils import JupyternetesUtils
from .clients import JupyterNotebookInstanceClient
from jupyterhub.traitlets import Unicode, Integer
from .models import V1JupyterNotebookInstance

class JupyternetesSpawner(Spawner):
    utils : JupyternetesUtils
    instance_client : JupyterNotebookInstanceClient

    template_name = Unicode(
        default_value="default",
        help = """
        The name of the template to use for this instance
        """
    ).tag(config=True)

    instance_name = Unicode(
        help = """
        The name of the instance being created
        """
    ).tag(config=True)

    instance_port = Integer(
        default_value=80,
        help = """
        The default instance port
        """
    ).tag(config=True)

    
    max_wait = Integer(
        default_value=300,
        help = """
        Max wait for an instance to be provisioned
        """
    ).tag(config=True)


    
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.log.debug("Jupyternetes Spawner initializing")
        self.utils = JupyternetesUtils(self)
        self.log.debug("Initializing JupyterNotebookInstanceClient")
        self.instance_client = JupyterNotebookInstanceClient(self)
        self.log.debug("Jupyternetes Spawner initialized")


    def load_state(self, state):
        """
        override from inherited class:

        Load the state of the spawner from the given state dictionary.
        """
        self.log.debug("Jupyternetes Spawner Loading State")
        super().load_state(state)
        self.instance_name = state["instance_name"]
        self.instance_namespace = state["instance_namespace"]

    def get_state(self):
        """
        override from inherited class:

        Get the state of the spawner as a dictionary.
        """
        self.log.debug("Jupyternetes Getting Spawner State")
        state = super().get_state()
        state["instance_name"] = self.instance_name
        state["instance_namespace"] = self.instance_namespace
        return state

    async def start(self):
        """
        override from inherited class:

        Start the spawner.
        """

        self.log.info("Starting Jupyternetes Spawner")
        return self.utils.start_instance()

        

    async def stop(self, now=False):
        """
        override from inherited class:

        Stop the spawner.
        """
        if not now:
            self.log.info("Stopping Jupyternetes Spawner")
            instance_list = await self.instance_client.list(self.instance_namespace, field_selector=f"metadata.name={self.instance_name}")
            if len(instance_list.items) > 0:
                self.log.info(f"Deleting instance: {self.instance_name} on namespace: {self.instance_namespace}")
                await self.instance_client.delete(self.instance_name, self.instance_namespace)
                self.log.info("Instance deleted")

    async def poll(self):
        """
        override from inherited class:

        Poll the spawner.
        """
        self.log.debug(f"Polling Spawner for {self.instance_name} in {self.instance_namespace}")
        instance_list = await self.instance_client.list(self.instance_namespace, field_selector=f"metadata.name={self.instance_name}")
        if len(instance_list.items) > 0:
            self.log.debug(f"Polling Returning None for {self.instance_name} in {self.instance_namespace}")
            return None
        
        self.log.debug(f"Polling Returning 0 for {self.instance_name} in {self.instance_namespace}")
        return 0