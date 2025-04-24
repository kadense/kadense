from pydantic import BaseModel, Field, ConfigDict
from typing import Optional
from .. import kubernetes
from .v1_jupyter_notebook_instance_spec import V1JupyterNotebookInstanceSpec
from .v1_jupyter_notebook_instance_status import V1JupyterNotebookInstanceStatus


class V1JupyterNotebookInstance(BaseModel):
    api_version : Optional[str] = Field(default = "kadense.io/v1", alias = "apiVersion")
    kind : Optional[str] = Field(default = "JupyterNotebookInstance", alias = "kind")
    metadata : Optional[kubernetes.V1ObjectMeta] = Field(default = None, alias = "metadata")
    spec : Optional[V1JupyterNotebookInstanceSpec] = Field(default = V1JupyterNotebookInstanceSpec(), alias = "spec")
    status : Optional[V1JupyterNotebookInstanceStatus] = Field(default = None, alias = "status")
