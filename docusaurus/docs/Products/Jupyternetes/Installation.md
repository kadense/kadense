---
title: Installing Jupyternetes
sidebar_position: 3
---


## Prerequisites
In order to install jupyternetes you will need to install and configure helm.

https://helm.sh/docs/intro/install/

Once installed you will need to add the helm charts for both helm and jupyter
```bash
helm repo add hitcs https://headinthecloudsolutions.github.io/helm-charts/
helm repo add jupyterhub https://hub.jupyter.org/helm-chart/                   
helm repo update
```

## Installing Kadense
```bash
helm upgrade --install kadense-crds hitcs/kadense-crds -n kadense --create-namespace
helm upgrade --install kadense hitcs/kadense -n kadense --create-namespace
```

## Installing Jupyter Hub
First you will need to create a basic configuration file:

```yaml config.yaml
hub:
  image:
    name: ghcr.io/headinthecloudsolutions/kadense/jupyternetes-hub
    tag: 0.1.134

  config:
    JupyterHub:
      spawner_class: jupyternetes_spawner.JupyternetesSpawner
      
scheduling:
  userScheduler:
    enabled: false
  userPlaceholder:
    enabled: false
```

Once you've got your config file you can then install jupyter hub using the zero-to-jupyterhub helm chart.

```bash
helm upgrade --install jupyterhub jupyterhub/jupyterhub -n jupyterhub --create-namespace
```

You'll need to give the jupyter hub service permissions to the JupyterNotebookInstances. To do this you can create a roles.yaml file:

```yaml roles.yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRole
metadata:
  name: jupyternetes-hub 
rules:
- apiGroups: [ "kadense.io" ]
  resources: ["jupyternotebookinstances", "jupyternotebooktemplates", "jupyternotebookinstances/status", "jupyternotebooktemplates/status"]
  verbs: [ "get", "list", "watch", "patch", "create", "delete" ]

- apiGroups: [""]
  resources: ["pods", "pods/status"]
  verbs: ["get", "list"]
---
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRoleBinding
metadata:
  name: jupyternetes-hub:hub
roleRef:
  apiGroup: rbac.authorization.k8s.io
  kind: ClusterRole
  name: jupyternetes-hub
subjects:
- kind: ServiceAccount
  name: hub
  namespace: jupyterhub
```

And then apply this using the following command:

```bash
kubectl apply -f ./roles.yaml
```

You can then expose the jupyterhub service by forwarding the port to your local machine:

```bash
 kubectl port-forward svc/proxy-public -n jupyterhub 8081:80
 ```
