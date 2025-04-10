from pydantic import BaseModel, Field, ConfigDict

from typing import Optional
from .v1_jupyter_notebook_instance import V1JupyterNotebookInstance
from .. import kubernetes


class V1JupyterNotebookInstanceList(BaseModel):
    model_config = ConfigDict(arbitrary_types_allowed = True)

    api_version : Optional[str] = Field(default = "kadense.io/v1", alias = "apiVersion")
    items : Optional[list[V1JupyterNotebookInstance]] = Field(default = [], alias = "items")
    kind : Optional[str] = Field(default = "JupyterNotebookInstanceList", alias = "kind")
    
    metadata : Optional[kubernetes.V1ListMeta] = Field(default = kubernetes.V1ListMeta(), alias = "metadata")