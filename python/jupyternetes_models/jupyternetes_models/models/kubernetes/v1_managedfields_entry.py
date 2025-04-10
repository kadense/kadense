from pydantic import BaseModel, Field, ConfigDict
from typing import Optional


class V1ManagedFieldsEntry(BaseModel):
    api_version : Optional[str] = Field(default = None, alias = "apiVersion")
    fields_type : Optional[str] = Field(default = None, alias = "fieldsType")
    fields_v1 : Optional[object] = Field(default = None, alias = "fieldsV1")
    manager : Optional[str] = Field(default = None, alias = "manager")
    operation : Optional[str] = Field(default = None, alias = "operation")
    subresource : Optional[str] = Field(default = None, alias = "subresource")
    time : Optional[str] = Field(default = None, alias = "time")    