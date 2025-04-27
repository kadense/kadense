---
sidebar_position: 4
title: Variables
---

The following variables are available for use in a JupyterNotebookTemplate by default:

| Parameter Name | Description |
| --- | --- |
| jupyterhub.user.id | The user id supplied by jupyter hub |
| jupyterhub.user.name | The username supplied by jupyter hub |
| jupyternetes.instance.name | The name of the JupyterNotebookInstance in kubernetes that this relates to |
| jupyternetes.instance.namespace | The namespace of the JupyterNotebookInstance in kubernetes that this relates to |
| jupyterhub.api_token | The API Token supplied to the pod that this notebook will use |
| jupyterhub.namespace | Namespace of jupyterhub |
| jupyterhub.oauth_client_id | The client id of jupyter hub |
| jupyternetes.pvcs.your_workspace_name | Gets the name of a Persistent Volume Claim (PVC) that workspaces needs to function. This will also tell jupyternetes to process the PVC first and to refer to it on the pod spec |
