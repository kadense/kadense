from ..models.jupyter.notebook.instance import (
    V1JupyterNotebookInstance,
    V1JupyterNotebookInstanceSpec,
    V1JupyterNotebookInstanceSpecTemplate
)
from ..models.kubernetes import V1ObjectMeta


class JupyterMockers:
    def mock_instance(self, name : str = "py-test", namespace : str = "default", template_name : str = "py-test", template_namespace : str = "default", resource_version = "811600"):
        metadata = V1ObjectMeta(
            name = name,
            namespace = namespace,
            labels = {
                'jupyternetes.kadense.io/test-label': 'test'
            },
            annotations= {
                'jupyternetes.kadense.io/test-annotation': 'test'

            },
        )

        return V1JupyterNotebookInstance(
            metadata= metadata,
            spec = V1JupyterNotebookInstanceSpec(
                template = V1JupyterNotebookInstanceSpecTemplate(
                    name = template_name,
                    namespace = template_namespace
                ),
                variables = {
                    "username" : "test-user",
                    "unescaped-username" : "test-user"
                }
            )
        )

    