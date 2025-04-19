from pydantic import BaseModel, Field, ConfigDict
from typing import Optional
from .v1_jupyter_notebook_instance_spec_template import V1JupyterNotebookInstanceSpecTemplate


class V1JupyterNotebookInstanceStatuPodsResourceState(BaseModel):
    resource_name : str = Field(default = "", alias = "resourceName")
    state : str = Field(default = "", alias = "state")
    error_message : Optional[str] = Field(default = "", alias = "errorMessage")
    parameters: Optional[dict[str, str]] = Field(default = None, alias = "parameters")
    pod_address : Optional[str] = Field(default = "", alias = "podAddress")
    port_number : Optional[int] = Field(default = "", alias = "portNumber")