from kubernetes_asyncio.client.models import V1ObjectMeta, V1Pod, V1PodSpec, V1Container, V1ContainerPort, V1PodStatus

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
