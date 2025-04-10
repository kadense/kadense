from ..utils import JupyternetesUtils
import pytest
from .mockers import Mocker

class TestJupyternetesUtils:
    def test_get_pod_url(self):
        mocker = Mocker()
        pod = mocker.mock_pod()

        utils = JupyternetesUtils()
        url = utils.get_pod_url(pod)
        assert url == "http://10.128.15.51:80"

    @pytest.mark.parametrize(
        "user_name, expected",
        [
            ("joe.bloggs@some.org", "4a517fbb40b458dfb86bcab50a510d07"),
            ("some.org\joe.bloggs", "6ae49e96cea35a248d9d3e2de668c8fc"),
            ("joe.bloggs-some-org", "e40f19916a3e5a29adfcfcd9eb1d33ef"),
            ("3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442881097566593344612847564823378678316527120190914564856692346034861045432664821339360726024914127372458700660631558817488152092096282925409171536436789259036001133053054882046652138414695194151160943305727036575959195309218611738193261179310511854807446237996274956735188575272489122793818301194912", "f37981d4d9cd51298a15ddcb86a28ef0"),
        ]
    )
    def test_get_unique_instance_name_email(self, user_name, expected):
        utils = JupyternetesUtils()
        unique_name = utils.get_unique_instance_name(user_name)
        assert unique_name == expected