from .mockers import JupyterMockers

class TestV1JupyterNotebookInstance:
    def test_instance(self):
        mockers = JupyterMockers()
        instance = mockers.mock_instance()

        assert instance.metadata.name == "py-test"
        assert instance.spec.template.name == "py-test"
