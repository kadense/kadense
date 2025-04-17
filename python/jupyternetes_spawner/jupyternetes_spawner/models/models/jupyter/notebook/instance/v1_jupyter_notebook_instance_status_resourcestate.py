from pydantic import BaseModel, Field, ConfigDict
from typing import Optional
from .v1_jupyter_notebook_instance_spec_template import V1JupyterNotebookInstanceSpecTemplate


class V1JupyterNotebookInstanceStatusResourceState(BaseModel):
    resource_name : str = Field(default = "", alias = "resourceName")
    state : str = Field(default = "", alias = "state")
    error_message : Optional[str] = Field(default = "", alias = "errorMessage")