from pydantic import BaseModel, Field, ConfigDict
from typing import Optional

class V1JupyterNotebookInstanceStatusPodResourceState(BaseModel):
    resource_name : str = Field(default = "", alias = "resourceName")
    state : str = Field(default = "", alias = "state")
    error_message : Optional[str] = Field(default = "", alias = "errorMessage")
    parameters : Optional[dict[str,str]] = Field(default = "", alias = "parameters")
    pod_address : Optional[str] = Field(default = "", alias = "podAddress")
    port_number : Optional[int] = Field(default = 80, alias = "portNumber")