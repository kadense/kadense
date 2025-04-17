from pydantic import BaseModel, Field, ConfigDict
from typing import Optional

class V1ListMeta(BaseModel):
    continue_value : Optional[str] = Field(default = None, alias = "continue")
    resource_version : Optional[str] = Field(default = None, alias = "resourceVersion")    
    self_link : Optional[str] = Field(default = None, alias = "selfLink")
    remaining_item_count : Optional[int] = Field(default = None, alias = "remainingItemCount")