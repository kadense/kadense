from pydantic import BaseModel, Field, ConfigDict
from typing import Optional
from .. import kubernetes
from .v1_jupyter_notebook_instance_spec import V1JupyterNotebookInstanceSpec


class V1JupyterNotebookInstance(BaseModel):
    model_config = ConfigDict(arbitrary_types_allowed = True)

    api_version : Optional[str] = Field(default = "kadense.io/v1", alias = "apiVersion")
    kind : Optional[str] = Field(default = "JupyterNotebookInstance", alias = "kind")
    metadata : Optional[kubernetes.V1ObjectMeta] = Field(default = None, alias = "metadata")
    spec : Optional[V1JupyterNotebookInstanceSpec] = Field(default = V1JupyterNotebookInstanceSpec(), alias = "spec")
