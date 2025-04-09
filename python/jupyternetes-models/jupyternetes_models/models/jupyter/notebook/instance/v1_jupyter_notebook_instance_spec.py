from pydantic import BaseModel, Field, ConfigDict
from typing import Optional
from .v1_jupyter_notebook_instance_spec_template import V1JupyterNotebookInstanceSpecTemplate


class V1JupyterNotebookInstanceSpec(BaseModel):
    model_config = ConfigDict(arbitrary_types_allowed = True)

    template : Optional[V1JupyterNotebookInstanceSpecTemplate] = Field(default = None, alias = "template")
    variables : Optional[dict[str,str]] = Field(default = {}, alias = "variables")
