from asyncio import sleep
from kubernetes_asyncio.client.models import V1ObjectMeta, V1Pod, V1PodSpec, V1Container, V1ContainerPort, V1PodStatus
from pydantic import TypeAdapter
from ..models import (
    V1JupyterNotebookInstance,
    V1JupyterNotebookInstanceList,
    V1JupyterNotebookInstanceSpec,
    V1JupyterNotebookInstanceSpecTemplate,
    V1JupyterNotebookInstanceStatus,
    V1JupyterNotebookInstanceStatusResourceState,
    PodResourceState,
    V1ListMeta,
    V1ObjectMeta as V1ObjectMetaKadense,
)
from ..utils import JupyternetesUtils

class MockSpawnerLog:
    def info(self, message: str):
        print(f"INFO: {message}")

    def error(self, message: str):
        print(f"ERROR: {message}")

    def debug(self, message: str):
        print(f"DEBUG: {message}")

class MockUser:
    name : str = "test-user"
    id : str = "1234"

class MockInstanceClient:
    logger = MockSpawnerLog()

    async def get(self, namespace, name):
        await sleep(0.1)  # Simulate async delay
        self.logger.debug(f"MockInstanceClient.get called with namespace: {namespace}, name: {name}")
        json = {
            "apiVersion": "kadense.io/v1",
            "kind": "JupyterNotebookInstance",
            "metadata": {
                "creationTimestamp": "2025-04-24T12:07:17Z",
                "generation": 1,
                "name": "8dc366d8a0f05869bfdb6e7eb3d83f65",
                "namespace": "default",
                "resourceVersion": "328533",
                "uid": "cecf55a9-be38-4452-9aa7-46e35db0c366"
            },
            "spec": {
                "template": {
                    "name": "default-template",
                    "namespace": "kadense"
                },
                "variables": {
                    "jupyterhub.user.id": "1",
                    "jupyterhub.user.name": "jovyan",
                    "jupyternetes.instance.name": "8dc366d8a0f05869bfdb6e7eb3d83f65",
                    "jupyternetes.instance.namespace": "default"
                }
            },
            "status": {
                "otherResources": {},
                "otherResourcesProvisioned": {},
                "pods": {
                    "test-container": {
                        "errorMessage": "",
                        "parameters": {},
                        "podAddress": "10.128.15.23",
                        "resourceName": f"{name}-p24",
                        "state": "Running"
                    }
                },
                "podsProvisioned": "Completed",
                "pvcs": {
                    "workspace": {
                        "parameters": {},
                        "resourceName": "workspace-fphcr",
                        "state": "Processed"
                    }
                },
                "pvcsProvisioned": "Completed"
            }
        }
        adapter = TypeAdapter(V1JupyterNotebookInstance)
        instance = adapter.validate_python(json, by_alias=True)

        print(instance)

        return instance
    
    async def list(self, namespace, field_selector = None, label_selector = None):
        return V1JupyterNotebookInstanceList(
            metadata=V1ListMeta(
                resourceVersion="811600"
            ),
            items=[
                await self.get(namespace, "py-test")
            ]
        )
        
    async def create(self, namespace, body : V1JupyterNotebookInstance):
        return await self.get(namespace, body.metadata.name)

class MockSpawner:
    user : MockUser
    template_name : str = "py-test"
    template_namespace : str = "default"
    instance_namespace : str = "default"
    utils : JupyternetesUtils
    log : MockSpawnerLog
    status_check_max_wait : int = 1
    api_token : str = "test-token"
    oauth_client_id : str = "test-client-id"

    def __init__(self):
        self.user = MockUser()
        self.log = MockSpawnerLog()
        self.utils = JupyternetesUtils(self)

class Mocker:
    """
    A class to mock objects for testing purposes.
    """
    def mock_pod(self, name: str = "py-test", namespace: str = "default", resource_version="811600"):
        pod = V1Pod(
            metadata=V1ObjectMeta(
                name=name,
                namespace=namespace,
                labels={
                    'jupyternetes.kadense.io/test-label': 'test'
                },
                annotations={
                    'jupyternetes.kadense.io/test-annotation': 'test'
                },
                resource_version=resource_version
            ),
            spec = V1PodSpec(
                containers=[
                    V1Container(
                        name="test-container",
                        image="test-image",
                        ports=[V1ContainerPort(container_port=8888)]
                    )
                ],
                
            ),
            status=V1PodStatus(
                pod_ip="10.128.15.51"
            )
        )
        return pod.to_dict(True)

    def mock_user(self):
        return MockUser()
    
    def mock_instance_client(self):
        return MockInstanceClient()
    
    def mock_spawner(self):
        spawner = MockSpawner()
        spawner.utils = JupyternetesUtils(spawner)
        spawner.instance_client = MockInstanceClient()
        return spawner