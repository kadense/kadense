---
slug: introducing-jupyternetes
title: Introducing Jupyternetes
authors: [shaun]
tags: [jupyternetes,intro]
---

Today it's my pleasure to introduce Jupyternetes, the first of a number of products that are built on the Kadense Framework.
<!-- truncate -->

Jupyternetes is an enterprise ready alternative for [kubespawner](https://github.com/jupyterhub/kubespawner) that uses the kubernetes operator model to provision the pods and persistent volume claims (PVC's) but is also left open to be extended for other operators.  This was [proposed as an amendment to the existing kubespawner](https://github.com/jupyterhub/kubespawner/issues/839#issuecomment-2102164475)

It allows administrators to template the resources that they want to be created .

```yaml
apiVersion: kadense.io/v1
kind: JupyterNotebookTemplate
metadata:
  name: default-template
  namespace: kadense
spec:
  pods:
  - labels:
      jupyternetes.kadense.io/userId: '{jupyterhub.user.id}'
    name: notebook
    spec:
      containers:
      - env:
        - name: JPY_API_TOKEN
          value: '{jupyterhub.api_token}'
        - name: JUPYTERHUB_ACTIVITY_URL
          value: http://hub.{jupyterhub.namespace}.svc.cluster.local:8081/hub/api/users/{jupyterhub.user.name}/activity
        - name: JUPYTERHUB_ADMIN_ACCESS
          value: "1"
        - name: JUPYTERHUB_API_TOKEN
          value: '{jupyterhub.api_token}'
        - name: JUPYTERHUB_API_URL
          value: http://hub.{jupyterhub.namespace}.svc.cluster.local:8081/hub/api
        - name: JUPYTERHUB_BASE_URL
          value: /
        - name: JUPYTERHUB_CLIENT_ID
          value: '{jupyterhub.oauth_client_id}'
        - name: JUPYTERHUB_COOKIE_HOST_PREFIX_ENABLED
          value: "0"
        - name: JUPYTERHUB_HOST
        - name: JUPYTERHUB_OAUTH_ACCESS_SCOPES
          value: '["access:servers!server={jupyterhub.user.name}/", "access:servers!user={jupyterhub.user.name}"]'
        - name: JUPYTERHUB_OAUTH_CALLBACK_URL
          value: /user/{jupyterhub.user.name}/oauth_callback
        - name: JUPYTERHUB_OAUTH_CLIENT_ALLOWED_SCOPES
          value: '[]'
        - name: JUPYTERHUB_OAUTH_SCOPES
          value: '["access:servers!server={jupyterhub.user.name}/", "access:servers!user={jupyterhub.user.name}"]'
        - name: JUPYTERHUB_PUBLIC_HUB_URL
        - name: JUPYTERHUB_PUBLIC_URL
        - name: JUPYTERHUB_SERVER_NAME
        - name: JUPYTERHUB_SERVICE_PREFIX
          value: /user/{jupyterhub.user.name}/
        - name: JUPYTERHUB_SERVICE_URL
          value: http://0.0.0.0:8888/user/{jupyterhub.user.name}/
        - name: JUPYTERHUB_USER
          value: '{jupyterhub.user.name}'
        - name: JUPYTER_IMAGE
          value: quay.io/jupyter/datascience-notebook:hub-5.2.1
        - name: JUPYTER_IMAGE_SPEC
          value: quay.io/jupyter/datascience-notebook:hub-5.2.1
        - name: MEM_GUARANTEE
          value: "1073741824"
        image: quay.io/jupyter/datascience-notebook:hub-5.2.1
        name: notebook
        ports:
        - containerPort: 8888
          name: http
          protocol: TCP
        volumeMounts:
        - mountPath: /home/{jupyterhub.user.name}/work
          name: workspace-pvc
      volumes:
      - name: workspace-pvc
        persistentVolumeClaim:
          claimName: '{jupyternetes.pvcs.workspace}'
  pvcs:
  - labels:
      jupyternetes.kadense.io/userId: '{jupyterhub.user.id}'
    name: workspace
    spec:
      accessModes:
      - ReadWriteOnce
      resources:
        requests:
          storage: 1Gi
```

This includes a number of variables enclosed in curly brackets, these are supplied by jupyterhub when it creates a JupyterNotebookInstance:

```yaml
apiVersion: kadense.io/v1
kind: JupyterNotebookInstance
metadata:
  name: 8dc366d8a0f05869bfdb6e7eb3d83f65
  namespace: default
spec:
  template:
    name: default-template
    namespace: kadense
  variables:
    jupyterhub.api_token: 852dc4bd4e4e4360b7ae94cffa4f1c76
    jupyterhub.namespace: kadense
    jupyterhub.oauth_client_id: jupyterhub-user-jovyan
    jupyterhub.user.id: "1"
    jupyterhub.user.name: jovyan
    jupyternetes.instance.name: 8dc366d8a0f05869bfdb6e7eb3d83f65
    jupyternetes.instance.namespace: default
status:
  otherResources: {}
  otherResourcesProvisioned: {}
  pods: {}
  podsProvisioned: Pending
  pvcs: {}
  pvcsProvisioned: Pending
```

As the template will accept any field that is supported by a V1PodSpec, it is much easier to make simple changes to your pod or PVC definitions without having to make customisations to jupyterhub itself.

In addition to this it jupyterhub itself does not need to have permissions to create workloads itself or amend the templates, meaning that if jupyterhub somehow gets breached then there are hard limitations on what they can do to the cluster itself via the jupyterhub service.