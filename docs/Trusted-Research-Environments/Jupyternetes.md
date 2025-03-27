# Jupyternetes
Jupyternetes is an alternative to Jupyterhub's Kubespawner built to the operator models and simplifying configuration.

This was [proposed as an amendment to the existing kubespawner](https://github.com/jupyterhub/kubespawner/issues/839#issuecomment-2102164475), however due to concerns regarding upgrade path's it was decided that this would be a separate project which will be managed by [Head in the Cloud Solutions Limited](https://www.headinthecloudsolutions.co.uk/) at least until the initial version is completed with an aim to bring this into the wider Project Jupyter when it works as expected.

Jupyternetes effectively breaks down the current hub down in kubernetes into multiple services and utilise the kubernetes operator design pattern and implement a more refined security policy for these services. 
