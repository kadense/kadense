---
sidebar_position: 1
title: Introduction to Jupyternetes
---

Jupyternetes is an alternative to [Jupyterhub's Kubespawner](https://github.com/jupyterhub/kubespawner) built on the operator model and designed to simplify configuration.

This was [proposed as an amendment to the existing Kubespawner](https://github.com/jupyterhub/kubespawner/issues/839#issuecomment-2102164475). However, due to concerns regarding upgrade paths, it was decided that this would be a separate project managed by [Head in the Cloud Solutions Limited](https://www.headinthecloudsolutions.co.uk/) at least until the initial version is completed. The aim is to eventually integrate it into the wider Project Jupyter once it meets expectations.

Jupyternetes effectively breaks down the current hub in Kubernetes into multiple services, utilizing the Kubernetes operator design pattern and implementing a more refined security policy for these services.

## The benefits

### More Secure
By splitting out the services, Jupyternetes refines the permissions of each service, giving them access only to what they need to perform their work. This adheres to the principle of least privilege within Kubernetes RBAC.

Keeping these services separate limits the potential damage if one service's security is breached. For example, if JupyterHub is breached, attackers cannot directly create pods or workloads. They would also need to exploit the Jupyternetes operators.

#### Additional Security
Additional validation can be applied to the operator services to reduce the opportunities for exploiting the solution.

### More Robust
By splitting the services, capabilities are isolated from one another. This means that a failure in one capability does not necessarily impact all users or templates.

### Easier to Amend Pods
While Kubespawner allows significant flexibility, it requires code changes for simple modifications to a pod definition. For instance, adding an environment variable to a pod involves amending custom code, testing the Python code, building the image, and uploading it to a repository. In Jupyternetes, the same change can be made in seconds by updating the template custom resource definition.

### Easier to Manage Templates
Adding, modifying, or removing templates is straightforward in Jupyternetes and can be done using the standard **kubectl** CLI.

#### Making it Even Easier
A management portal is planned and will be provided if there is sufficient demand within the community.

#### Highly Extensible
The JupyterNotebookInstance Custom Resource Definition allows for the inclusion of resources beyond the standard PVC and Pods. It also supports adding statuses. This makes it easy to template new operators for various purposes, resulting in high extensibility.
