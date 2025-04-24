import ipaddress
import re
from asyncio import sleep
from uuid import UUID, uuid5
from .models import (
        V1JupyterNotebookInstance,
        V1JupyterNotebookInstanceSpec,
        V1JupyterNotebookInstanceSpecTemplate,
        V1JupyterNotebookInstanceList,
        V1JupyterNotebookInstanceStatus,
        V1ObjectMeta,
        V1JupyterNotebookInstanceStatusPodResourceState
)

class JupyternetesUtils:
    def __init__(self, spawner=None):
        spawner.log.debug("JupyternetesUtils initializing")
        self.spawner = spawner
        self.non_alphanum_pattern = re.compile(r'[^a-zA-Z0-9]+')
        self.default_uuid = UUID("00000000-0000-0000-0000-000000000000")
        spawner.log.debug("JupyternetesUtils initialized")

    def get_unique_instance_name(self, name: str) -> str:
        """
        Generate a unique instance name from the user name
        """
        
        result = uuid5(self.default_uuid, name).hex
        self.spawner.log.debug(f"get_unique_instance_name: {result}")

        return result
    
    def get_pod_url(self, pod):
        """Return the pod url

        Default: use pod.status.pod_ip (dns_name if ssl or services_enabled is enabled)
        """

        proto = "http"
        hostname = pod["status"]["podIP"]

        if isinstance(ipaddress.ip_address(hostname), ipaddress.IPv6Address):
            hostname = f"[{hostname}]"
        
        port: int = pod["spec"]["containers"][0]["ports"][0]["containerPort"]
        result = "{}://{}:{}".format(
            proto,
            hostname,
            port,
        )

        self.spawner.log.debug(f"get_pod_url: {result}")
        return result
    
    def create_instance(self, instance_name : str = None):
        """
        Create a instance from the details provided by the spawner
        """
        if instance_name is None:
            instance_name = self.get_unique_instance_name(self.spawner.user.name)

        instance = V1JupyterNotebookInstance(
            metadata = V1ObjectMeta(
                name = instance_name,
                namespace = self.get_instance_namespace(),
            ),
            spec = V1JupyterNotebookInstanceSpec(
                template = V1JupyterNotebookInstanceSpecTemplate(
                    name = self.get_template_name(),
                    namespace = self.get_template_namespace(),
                ),
                variables = self.get_instance_variables()
            )
        )
        return instance

    def get_instance_variables(self):
        """
        Get the instance variables from the spawner
        """
        return {
            "jupyterhub.user.id" : str(self.spawner.user.id),
            "jupyterhub.user.name" : self.spawner.user.name,
            "jupyternetes.instance.name" : self.get_unique_instance_name(self.spawner.user.name),
            "jupyternetes.instance.namespace" : self.get_instance_namespace(),
        }
    
    def get_template_name(self):
        """
        Get the template name from the spawner
        """
        return self.spawner.template_name
    
    def get_template_namespace(self):
        """
        Get the template name from the spawner
        """
        return self.spawner.template_namespace
    
    def get_instance_namespace(self):
        """
        Get the instance namespace from the spawner
        """
        return self.spawner.instance_namespace
        
    def get_pod_details(self, instance : V1JupyterNotebookInstance) -> tuple[str, int]:
        """
        Get the pod details from the pod object
        """

        if len(instance.status.pods) == 0:
            raise Exception("No pods found in instance status")

        for pod_key in instance.status.pods: 
            self.spawner.log.debug(f"Getting Pod Details for pod key: {pod_key}")
            pod = instance.status.pods[pod_key]
            self.spawner.log.debug(f"pod_address: {pod.pod_address}") 
            self.spawner.log.debug(f"port_number: {pod.port_number}")
            self.spawner.log.debug(f"resource_name: {pod.resource_name}")

            return [pod.pod_address, pod.port_number]

    async def start_instance(self) -> tuple[str, int]:
        instance : V1JupyterNotebookInstance = self.create_instance()
        self.spawner.instance_name = instance.metadata.name
        self.spawner.log.info(f"Checking if {instance.metadata.name} in {instance.metadata.namespace} currently exists")
        exists, instance = await self.check_instance_exists(instance)   
        if not exists:
            self.spawner.log.info(f"Creating instance {instance.metadata.name} in {instance.metadata.namespace}")
            instance = await self.spawner.instance_client.create(instance.metadata.namespace, instance)

        ready = self.check_instance_status(instance)

        wait : int = 1
        while ready == False and wait < self.spawner.max_wait:
            self.spawner.log.debug(f"instance {instance.metadata.name} in {instance.metadata.namespace} is not ready waiting {wait} seconds")
            await sleep(wait)
            instance = await self.spawner.instance_client.get(namespace = instance.metadata.namespace, name = instance.metadata.name)
            ready = self.check_instance_status(instance)
            wait = wait * 2
        
        return self.get_pod_details(instance)        

    async def check_instance_exists(self, instance : V1JupyterNotebookInstance) -> tuple[bool, V1JupyterNotebookInstance]:
        existing_instances = await self.spawner.instance_client.list(instance.metadata.namespace, field_selector=f"metadata.name={instance.metadata.name}")
        if len(existing_instances.items) > 0:
            self.spawner.log.info(f"Instance {instance.metadata.name} already exists")
            return [True, existing_instances.items[0]]

        return [False, instance]
    
    def check_instance_status(self, instance : V1JupyterNotebookInstance) -> bool:
        self.spawner.log.debug(f"Checking status of {instance.metadata.name} on {instance.metadata.namespace}")
        if instance.status:
            self.spawner.log.debug(f"Checking  {instance.metadata.name}")
            if instance.status.podsProvisioned and instance.status.podsProvisioned == "Processed":
                self.spawner.log.debug(f"Pods related to {instance.metadata.name} are provisioned")
                if instance.status.pods:
                    for pod_key in instance.status.pods:
                        self.spawner.log.debug(f"Pod {pod_key} related to {instance.metadata.name} are not not defined")
                        pod = instance.status.pods[pod_key]
                        if pod.state and pod.state == "Ready":
                            self.spawner.log.debug(f"Pod {pod_key} : {pod.resource_name} related to {instance.metadata.name} is ready")
                            return True
                        else:
                            self.spawner.log.debug(f"Pod {pod_key} : {pod.resource_name} related to {instance.metadata.name} is {pod.state}")
                else:
                    self.spawner.log.debug(f"Pods related to {instance.metadata.name} are not not defined")
            else:
                self.spawner.log.debug(f"Pods related to {instance.metadata.name} are not provisioned")
        else:
            self.spawner.log.debug(f"Instance {instance.metadata.name} is not defined")
        self.spawner.log.debug(f"Instance {instance.metadata.name} is not ready")
        return False
        
    
        