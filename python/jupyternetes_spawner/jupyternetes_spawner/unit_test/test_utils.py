from ..utils import JupyternetesUtils
import pytest
from .mockers import Mocker
from ..spawner import JupyternetesSpawner

class TestJupyternetesUtils:
    def test_get_pod_url(self):
        mocker = Mocker()
        pod = mocker.mock_pod()
        spawner = mocker.mock_spawner()

        url = spawner.utils.get_pod_url(pod)
        assert url == "http://10.128.15.51:8888"

    @pytest.mark.parametrize(
        "user_name, expected",
        [
            ("joe.bloggs@some.org", "4a517fbb40b458dfb86bcab50a510d07"),
            ("some.org\\joe.bloggs", "6ae49e96cea35a248d9d3e2de668c8fc"),
            ("joe.bloggs-some-org", "e40f19916a3e5a29adfcfcd9eb1d33ef"),
            ("3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442881097566593344612847564823378678316527120190914564856692346034861045432664821339360726024914127372458700660631558817488152092096282925409171536436789259036001133053054882046652138414695194151160943305727036575959195309218611738193261179310511854807446237996274956735188575272489122793818301194912", "f37981d4d9cd51298a15ddcb86a28ef0"),
        ]
    )
    def test_get_unique_instance_name_email(self, user_name, expected):
        mocker = Mocker()
        spawner = mocker.mock_spawner()
        unique_name = spawner.utils.get_unique_instance_name(user_name)
        assert unique_name == expected

    def test_get_instance_variables(self):
        mocker = Mocker()
        spawner = mocker.mock_spawner()
        variables = spawner.utils.get_instance_variables()
        assert variables["jupyterhub.user.id"] == spawner.user.id
        assert variables["jupyterhub.user.name"] == spawner.user.name
        assert variables["jupyternetes.instance.name"] == "ebf60aed2fea54fba7f249898ad18b8c"
        assert variables["jupyternetes.instance.namespace"] == spawner.instance_namespace

    def test_create_instance(self):
        mocker = Mocker()
        spawner = mocker.mock_spawner()
        instance = spawner.utils.create_instance()
        assert instance.metadata.namespace == spawner.instance_namespace
        assert instance.spec.template.name == spawner.template_name
        assert instance.metadata.name == "ebf60aed2fea54fba7f249898ad18b8c"
        assert instance.spec.variables["jupyterhub.user.id"] == spawner.user.id
        assert instance.spec.variables["jupyterhub.user.name"] == spawner.user.name
        assert instance.spec.variables["jupyternetes.instance.name"] == "ebf60aed2fea54fba7f249898ad18b8c"
        assert instance.spec.variables["jupyternetes.instance.namespace"] == spawner.instance_namespace

    @pytest.mark.asyncio
    async def test_check_instance_status(self):
        mocker = Mocker()
        spawner = mocker.mock_spawner()
        instance = await mocker.mock_instance_client().get("default", "ebf60aed2fea54fba7f249898ad18b8c")
        status = spawner.utils.check_instance_status(instance)
        assert status == True

    @pytest.mark.asyncio
    async def test_start_instance(self):
        mocker = Mocker()
        spawner = mocker.mock_spawner()
        pod_address, port_number = await spawner.utils.start_instance()
        assert pod_address == "10.128.15.23"
        assert port_number == 8888