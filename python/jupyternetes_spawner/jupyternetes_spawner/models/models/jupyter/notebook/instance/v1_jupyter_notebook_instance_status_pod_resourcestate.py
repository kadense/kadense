from pydantic import BaseModel, Field, ConfigDict
from typing import Optional

class PodResourceState(BaseModel):
    resourceName : Optional[str] = Field(default = "", alias = "resourceName")
    state : Optional[str] = Field(default = "", alias = "state")
    errorMessage : Optional[str] = Field(default = "", alias = "errorMessage")
    parameters : Optional[dict[str,str]] = Field(default = "", alias = "parameters")
    podAddress : Optional[str] = Field(default = "", alias = "podAddress")
    portNumber : Optional[int] = Field(default = 8888, alias = "portNumber")