import ipaddress
import re
from uuid import UUID, uuid5
from jupyternetes_models import (
        V1JupyterNotebookInstance,
        V1JupyterNotebookInstanceSpec,
        V1JupyterNotebookInstanceSpecTemplate,
        V1JupyterNotebookInstanceList,
        V1ObjectMeta
)

class JupyternetesUtils:
    def __init__(self):
        self.non_alphanum_pattern = re.compile(r'[^a-zA-Z0-9]+')
        self.default_uuid = UUID("00000000-0000-0000-0000-000000000000")

    def get_unique_instance_name(self, spawner,  name: str) -> str:
        """
        Generate a unique instance name from the user name
        """
        
        spawner.log.debug(f"get_unique_instance_name: {name}")
        return uuid5(self.default_uuid, name).hex
    
    def get_pod_url(self, spawner, pod):
        """Return the pod url

        Default: use pod.status.pod_ip (dns_name if ssl or services_enabled is enabled)
        """

        proto = "http"
        hostname = pod["status"]["podIP"]

        if isinstance(ipaddress.ip_address(hostname), ipaddress.IPv6Address):
            hostname = f"[{hostname}]"
        
        port: int = pod["spec"]["containers"][0]["ports"][0]["containerPort"]

        return "{}://{}:{}".format(
            proto,
            hostname,
            port,
        )
    
    def create_instance(self, spawner, instance_name : str, template_name : str):
        """
        Create a instance from the details provided by the spawner
        """

        instance = V1JupyterNotebookInstance(
            metadata = V1ObjectMeta(
                name = instance_name,
                namespace = spawner.get_instance_namespace(spawner),
            ),
            spec = V1JupyterNotebookInstanceSpec(
                template = V1JupyterNotebookInstanceSpecTemplate(
                    name = spawner.get_template_name(spawner),
                ),
                variables = spawner.get_instance_variables(spawner)
            )
        )
        return instance

    def get_instance_variables(self, spawner):
        """
        Get the instance variables from the spawner
        """
        return {
            "jupyterhub.user.name" : spawner.user.name,
            "jupyternetes.instance.name" : spawner.get_unique_instance_name(spawner),
            "jupyternetes.instance.namespace" : spawner.get_instance_namespace(spawner),
        }
    
    def get_template_name(self, spawner):
        """
        Get the template name from the spawner
        """
        return spawner.template_name
    
    def get_instance_namespace(self, spawner):
        """
        Get the instance namespace from the spawner
        """
        return spawner.instance_namespace