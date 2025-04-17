from ...models import (
    V1JupyterNotebookInstance,
    V1JupyterNotebookInstanceList
)

from kubernetes_asyncio.client import CustomObjectsApi
from kubernetes_asyncio.client.exceptions import ApiException
from logging import Logger, getLogger
from .kubernetes_client import KubernetesNamespacedCustomClient

class JupyterNotebookInstanceClient(KubernetesNamespacedCustomClient):
    def __init__(self, configuration=None, header_name=None, header_value=None, cookie=None, pool_threads : int = 1):
        logger = getLogger(__name__)
        super().__init__(
            log = logger, 
            group = "kadense.io", 
            version = "v1", 
            plural = "jupyternotebookinstances", 
            kind = "JupyterNotebookInstance",
            list_type = V1JupyterNotebookInstanceList,
            singleton_type = V1JupyterNotebookInstance,
            configuration=configuration,
            header_name=header_name,
            header_value=header_value,
            cookie=cookie,
            pool_threads=pool_threads
            )