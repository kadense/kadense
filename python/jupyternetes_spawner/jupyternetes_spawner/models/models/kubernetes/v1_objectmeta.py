from pydantic import BaseModel, Field, ConfigDict
from typing import Optional
from .v1_managedfields_entry import V1ManagedFieldsEntry
from .v1_owner_reference import V1OwnerReference


class V1ObjectMeta(BaseModel):
    annotations : Optional[dict[str,str]] = Field(default = None, alias = "annotations")
    creation_timestamp : Optional[str] = Field(default = None, alias = "creationTimestamp")
    deletion_grace_period_seconds : Optional[int] = Field(default = None, alias = "deletionGracePeriodSeconds")
    deletion_timestamp : Optional[str] = Field(default = None, alias = "deletionTimestamp")
    finalizers : Optional[list[str]] = Field(default = None, alias = "finalizers")
    generate_name : Optional[str] = Field(default = None, alias = "generateName")
    generation : Optional[int] = Field(default = None, alias = "generation")
    labels : Optional[dict[str,str]] = Field(default = None, alias = "labels") 
    managed_fields : Optional[list[V1ManagedFieldsEntry]] = Field(default = None, alias = "managedFields")
    name : Optional[str] = Field(default = None, alias = "name")
    namespace : Optional[str] = Field(default = None, alias = "namespace")
    owner_references : Optional[list[V1OwnerReference]] = Field(default = None, alias = "ownerReferences")
    resource_version : Optional[str] = Field(default = None, alias = "resourceVersion")
    self_link : Optional[str] = Field(default = None, alias = "selfLink")
    uid : Optional[str] = Field(default = None, alias = "uid")
    