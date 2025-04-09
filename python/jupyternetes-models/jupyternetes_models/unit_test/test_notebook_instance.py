from .mockers import JupyterMockers
from ..models.jupyter.notebook.instance import V1JupyterNotebookInstance

class TestV1JupyterNotebookInstance:
    def test_instance(self):
        mockers = JupyterMockers()
        instance = mockers.mock_instance()

        assert instance.metadata.name == "py-test"
        assert instance.spec.template.name == "py-test"
