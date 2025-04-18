from logging import Logger
from pydantic import TypeAdapter, BaseModel
from kubernetes_asyncio.client import ApiClient
from kubernetes_asyncio.client.exceptions import ApiException
from typing import TypeVar, Generic, Mapping
import json
from kubernetes_asyncio.client.rest import RESTClientObject

class KubernetesNamespacedCustomClient(ApiClient):
    def __init__(self, log : Logger, group : str, version : str, plural : str, kind : str, list_type : type, singleton_type : type, configuration =None, header_name=None, header_value=None, cookie=None, pool_threads : int =1):
        log.debug(f"Initializing KubernetesNamespacedCustomClient with group: {group}, version: {version}, plural: {plural}, kind: {kind}")
        super().__init__(configuration, header_name, header_value, cookie, pool_threads)
        self.group = group
        self.version = version
        self.plural = plural
        self.kind = kind
        self.log : Logger = log

        self.list_adapter = TypeAdapter(list_type)

        self.singleton_adapter = TypeAdapter(singleton_type)
        log.debug(f"Initialized KubernetesNamespacedCustomClient with group: {group}, version: {version}, plural: {plural}, kind: {kind}")


    def get_api_version(self):
        return f"{self.group}/{self.version}"

    async def get(self, namespace, name):
        resource_path = f"/apis/{self.group}/{self.version}/namespaces/{namespace}/{self.plural}/{name}"
        response = await self.get_from_kube_api(resource_path)
        return self.singleton_adapter.validate_json(response)
    
    async def list(self, namespace, label_selector: str = None, field_selector: str = None):
        resource_path = f"/apis/{self.group}/{self.version}/namespaces/{namespace}/{self.plural}"
        response = await self.get_from_kube_api(resource_path, label_selector, field_selector)
        return self.list_adapter.validate_json(response)

    async def replace(self, namespace : str, name : str, body):
        resource_path = f"/apis/{self.group}/{self.version}/namespaces/{namespace}/{self.plural}/{name}"
        return await self.send_to_kube_api("PUT", resource_path, body)
     
    async def create_or_replace(self, namespace : str, name : str, body):
        try:
            existing_body = await self.get(namespace, name)
            
            existing_resource_version = existing_body.metadata.resource_version 
            body.metadata.resource_version = existing_resource_version 

            
            result = await self.replace(
                namespace = namespace,
                name = name,
                body = body
            )
            return result


        except ApiException as e:
            if e.status != 404:
                raise e 
            
            result = await self.create(
                namespace = namespace,
                body = body
            )
            return result

    
    async def create(self, namespace : str, body):
        resource_path = f"/apis/{self.group}/{self.version}/namespaces/{namespace}/{self.plural}"
        return await self.send_to_kube_api("POST", resource_path, body)
            
    
    async def delete(self, namespace : str, name : str):
        resource_path = f"/apis/{self.group}/{self.version}/namespaces/{namespace}/{self.plural}/{name}"
        await self.send_to_kube_api("DELETE", resource_path, None)

    
    
    async def get_from_kube_api(self, resource_path: str, label_selector: str = None, field_selector: str = None, watch : bool = False, resource_version : str = None, auth_settings = ['BearerToken']):
        headers = {'Content-Type': 'application/json'}
        
        query_params = []

        if label_selector:
            query_params.append(('labelSelector', label_selector))
            
        if field_selector:
            query_params.append(('fieldSelector', field_selector))

        if resource_version:
            query_params.append(('resourceVersion', resource_version))

        if watch:
            query_params.append(('watch', "1"))
        
        self.update_params_for_auth(headers, query_params, auth_settings)
        endpoint = self.configuration.host + resource_path
        response = await self.rest_client.request("GET", endpoint, headers=headers, query_params=query_params)
        if response.status != 200:
            self.log.error(f"Failed to GET to {endpoint}: {response.data}")
            response.raise_for_status()
        
        return response.data.decode("utf-8")
        
    
    async def send_to_kube_api(self, method : str, resource_path: str, model_instance: BaseModel, auth_settings = ['BearerToken']):
        self.log.debug(f"Sending {method} request to {resource_path} with body: {model_instance}")
        headers = {'Content-Type': 'application/json'}
        query_params = []
        
        self.update_params_for_auth(headers, query_params, auth_settings)
        body = self.singleton_adapter.dump_python(model_instance, exclude_none=True, by_alias=True)
        endpoint = self.configuration.host + resource_path
        response = await self.rest_client.request(method, endpoint, headers=headers, query_params=query_params, body=body)

        self.log.debug(f"{method} request to {resource_path} responsed with status: {response.status}")
        if not 200 <= response.status <= 299:
            raise ApiException(http_resp=response)
        
        return self.singleton_adapter.validate_json(response.data.decode("utf-8"))
        
