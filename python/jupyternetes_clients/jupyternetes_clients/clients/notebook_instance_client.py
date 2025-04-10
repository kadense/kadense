from jupyternetes_models import (
    V1JupyterNotebookInstance,
    V1JupyterNotebookInstanceList
)

from kubernetes_asyncio.client import CustomObjectsApi
from kubernetes_asyncio.client.exceptions import ApiException
from logging import Logger
from .kubernetes_client import KubernetesNamespacedCustomClient

class JupyterNotebookInstanceClient(KubernetesNamespacedCustomClient):
    def __init__(self, log: Logger):
        super().__init__(
            log = log, 
            group = "kadense.io", 
            version = "v1", 
            plural = "jupyternotebookinstances", 
            kind = "JupyterNotebookInstance",
            list_type = V1JupyterNotebookInstanceList,
            singleton_type = V1JupyterNotebookInstance
            )