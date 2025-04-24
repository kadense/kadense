from pydantic import BaseModel, Field, ConfigDict
from typing import Optional
from .v1_jupyter_notebook_instance_spec_template import V1JupyterNotebookInstanceSpecTemplate
from .v1_jupyter_notebook_instance_status_resourcestate import V1JupyterNotebookInstanceStatusResourceState
from .v1_jupyter_notebook_instance_status_pod_resourcestate import PodResourceState

class V1JupyterNotebookInstanceStatus(BaseModel):
    pvcs : Optional[dict[str,V1JupyterNotebookInstanceStatusResourceState]] = Field(default = None, alias = "pvcs")
    pods : Optional[dict[str,PodResourceState]] = Field(default = None, alias = "pods")
    otherResources : Optional[dict[str,V1JupyterNotebookInstanceStatusResourceState]] = Field(default = None, alias = "otherResources")
    podsProvisioned : Optional[str] = Field(default = None, alias = "podsProvisioned")
    pvcsProvisioned : Optional[str] = Field(default = None, alias = "pvcsProvisioned")
    otherResourcesProvisioned : Optional[dict[str, str]] = Field(default = None, alias = "otherResourcesProvisioned")
    