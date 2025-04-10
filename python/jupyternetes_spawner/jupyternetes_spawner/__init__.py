"""
Jupyternetes Spawner to spawn user notebooks on a Kubernetes cluster.

After installation, you can enable it by adding::

    c.JupyterHub.spawner_class = 'jupyternetes.JupyternetesSpawner'

in your `jupyterhub_config.py` file.
"""

# We export Jupyternetes specifically here. This simplifies import for users.
from .spawner import JupyternetesSpawner
