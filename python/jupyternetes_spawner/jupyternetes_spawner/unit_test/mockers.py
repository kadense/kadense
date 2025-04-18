from asyncio import sleep
from kubernetes_asyncio.client.models import V1ObjectMeta, V1Pod, V1PodSpec, V1Container, V1ContainerPort, V1PodStatus
from ..models import (
    V1JupyterNotebookInstance,
    V1JupyterNotebookInstanceList,
    V1JupyterNotebookInstanceSpec,
    V1JupyterNotebookInstanceSpecTemplate,
    V1JupyterNotebookInstanceStatus,
    V1JupyterNotebookInstanceStatusResourceState,
    V1JupyterNotebookInstanceStatusPodResourceState,
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

        pod_status = V1JupyterNotebookInstanceStatusPodResourceState()
        pod_status.resource_name = f"{name}-p24",
        pod_status.state = "Processed",
        pod_status.pod_address = "10.128.15.23"
        
        return V1JupyterNotebookInstance(
            metadata=V1ObjectMetaKadense(
                name=name,
                namespace=namespace,
                labels={
                    'jupyternetes.kadense.io/test-label': 'test'
                },
                annotations={
                    'jupyternetes.kadense.io/test-annotation': 'test'
                },
                resource_version="811600"
            ),
            spec = V1JupyterNotebookInstanceSpec(
                template=V1JupyterNotebookInstanceSpecTemplate(
                    name="py-test",
                ),
                variables={
                    "jupyterhub.user.id": "1234",
                    "jupyterhub.user.name": "test-user",
                    "jupyternetes.instance.name": "ebf60aed2fea54fba7f249898ad18b8c",
                    "jupyternetes.instance.namespace": "default"
                }
            ),
            status=V1JupyterNotebookInstanceStatus(
                pods={
                    "test-container": pod_status
                },
                podsProvisioned="Processed"
            )
        )
    
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
    max_wait : int = 30

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
                        ports=[V1ContainerPort(container_port=80)]
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