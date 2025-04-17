from pathlib import Path
from setuptools import setup

# This is where you add any fancy path resolution to the local lib:
jupyternetes_models_path: str = (Path(__file__).parent / "jupyternetes_models").as_uri()

setup(
    install_requires=[
        "jupyternetes-models @ {jupyternetes_models_path}",
        "pydantic",
        "kubernetes-asyncio",
        "pytz",
        'importlib-metadata; python_version<"3.10"'
    ]
)