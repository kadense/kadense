import os
import socket

c.JupyterHub.spawner_class = 'jupyternetes_spawner.JupyternetesSpawner'

c.JupyterHub.ip = '127.0.0.1'
c.JupyterHub.hub_ip = '127.0.0.1'

# Don't try to cleanup servers on exit - since in general for k8s, we want
# the hub to be able to restart without losing user containers
c.JupyterHub.cleanup_servers = False

if os.environ.get("CI"):
    # In the CI system we use k3s which will be accessible on localhost.
    c.JupyterHub.hub_connect_ip = "127.0.0.1"
else:
    # Find the IP of the machine that minikube is most likely able to talk to
    # Graciously used from https://stackoverflow.com/a/166589
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.connect(("8.8.8.8", 80))
    host_ip = s.getsockname()[0]
    s.close()

    c.JupyterHub.hub_connect_ip = host_ip

# Simplify testing by using a dummy authenticator class where any username
# password combination will work and where we don't provide persistent storage.
c.JupyterHub.authenticator_class = 'dummy'
c.JupyterHub.allow_named_servers = True
