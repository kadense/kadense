---
title: Installing Jupyternetes
sidebar_position: 3
---

## Prerequisites
To install Jupyternetes, you need to install and configure Helm. Follow the instructions at:

https://helm.sh/docs/intro/install/

After installation, add the Helm charts for both Kadense and JupyterHub by running the following commands in your terminal:

```bash
helm repo add hitcs https://kadense.github.io/helm-charts/
helm repo add jupyterhub https://hub.jupyter.org/helm-chart/                   
helm repo update
```

## Installing Kadense
Run the following commands to install Kadense:

```bash
helm upgrade --install kadense-crds hitcs/kadense-crds -n kadense --create-namespace
helm upgrade --install kadense hitcs/kadense -n kadense --create-namespace
```

## Installing JupyterHub
First, create a basic configuration file named `config.yaml` with the following content:

```yaml
hub:
  image:
    name: ghcr.io/kadense/kadense/jupyternetes-hub
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

Once you've created the configuration file, install JupyterHub using the zero-to-jupyterhub Helm chart:

```bash
helm upgrade --install jupyterhub jupyterhub/jupyterhub -n jupyterhub --create-namespace
```

Next, grant the JupyterHub service permissions to manage JupyterNotebookInstances. Create a file named `roles.yaml` with the following content:

```yaml
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

Apply the roles using the following command:

```bash
kubectl apply -f ./roles.yaml
```

Finally, expose the JupyterHub service by forwarding the port to your local machine:

```bash
kubectl port-forward svc/proxy-public -n jupyterhub 8081:80
```
