from pydantic import BaseModel, Field, ConfigDict
from typing import Optional


class V1JupyterNotebookInstanceSpecTemplate(BaseModel):
    model_config = ConfigDict(arbitrary_types_allowed = True)

    name : str = Field(default = "", alias = "name")
    namespace : Optional[str] = Field(default = "", alias = "namespace")