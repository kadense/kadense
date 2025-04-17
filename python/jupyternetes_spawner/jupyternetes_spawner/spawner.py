import jupyterhub
from jupyterhub.spawner import Spawner
from jupyterhub.traitlets import Callable, Integer, Command, Unicode
from jupyterhub.utils import exponential_backoff, maybe_future
from .utils import JupyternetesUtils
from .clients import JupyterNotebookInstanceClient

class JupyternetesSpawner(Spawner):
    utils : JupyternetesUtils
    instance_client : JupyterNotebookInstanceClient
    get_pod_url : Callable
    get_unique_instance_name : Callable 
    create_instance : Callable
    get_instance_variables : Callable 
    get_template_name : Callable 
    get_instance_namespace : Callable 

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


    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.utils = JupyternetesUtils()
        self.instance_client = JupyterNotebookInstanceClient()

        self.get_pod_url = Callable(
            default_value= self.utils.get_pod_url,
            allow_none=True,
            config=True,
            help="""Callable to retrieve pod url

            Called with (spawner, pod) returns str

            Must not be async
            """,
        )

        self.get_unique_instance_name = Callable(
            default_value=self.utils.get_unique_instance_name,
            allow_none=True,
            config=True,
            help="""Callable to retrieve pod url

            Called with (spawner, pod) returns str

            Must not be async
            """,
        )

        self.create_instance = Callable(
            default_value=self.utils.create_instance,
            allow_none=True,
            config=True,
            help="""Creates a instance from the details provided by the spawner

            Called with (spawner, instance_name, template_name) returns V1JupyterNotebookInstance

            Must not be async
            """,
        )

        self.get_instance_variables = Callable(
            default_value=self.utils.get_instance_variables,
            allow_none=True,
            config=True,
            help="""Creates a map of string variables to be passed to the template for this instance

            Called with (spawner) returns dict[str,str]

            Must not be async
            """,
        )

        self.get_template_name = Callable(
            default_value=self.utils.get_template_name,
            allow_none=True,
            config=True,
            help="""Gets the name of the template

            Called with (spawner) returns str

            Must not be async
            """,
        )

        self.get_instance_namespace = Callable(
            default_value=self.utils.get_instance_namespace,
            allow_none=True,
            config=True,
            help="""Gets the name of namespace where this instance will be created

            Called with (spawner) returns str

            Must not be async
            """,
        )


    def load_state(self, state):
        """
        override from inherited class:

        Load the state of the spawner from the given state dictionary.
        """
        pass

    def get_state(self):
        """
        override from inherited class:

        Get the state of the spawner as a dictionary.
        """
        state = super().get_state()
        return state




    def start(self):
        """
        override from inherited class:

        Start the spawner.
        """

        instance_name = self.get_unique_instance_name(self, self.user.name)
        
        self.log.info(f"Creating : {instance_name}")

        

        return self.start()

    async def stop(self, now=False):
        """
        override from inherited class:

        Stop the spawner.
        """
        pass

    async def poll(self):
        """
        override from inherited class:

        Poll the spawner.
        """
        return 1