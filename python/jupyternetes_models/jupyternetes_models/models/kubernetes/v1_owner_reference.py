from pydantic import BaseModel, Field, ConfigDict
from typing import Optional


class V1OwnerReference(BaseModel):
    api_version : Optional[str] = Field(default = None, alias = "apiVersion")
    block_owner_deletion : Optional[bool] = Field(default = None, alias = "blockOwnerDeletion")
    controller : Optional[bool] = Field(default = None, alias = "controller")
    kind : Optional[str] = Field(default = None, alias = "kind")
    name : Optional[str] = Field(default = None, alias = "name")
    uid : Optional[str] = Field(default = None, alias = "uid")