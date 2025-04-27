---
sidebar_position: 1
title: Introduction to Jupyternetes
---

Jupyternetes is an alternative to [Jupyterhub's Kubespawner](https://github.com/jupyterhub/kubespawner) built to the operator models and simplifying configuration.

This was [proposed as an amendment to the existing kubespawner](https://github.com/jupyterhub/kubespawner/issues/839#issuecomment-2102164475), however due to concerns regarding upgrade path's it was decided that this would be a separate project which will be managed by [Head in the Cloud Solutions Limited](https://www.headinthecloudsolutions.co.uk/) at least until the initial version is completed with an aim to bring this into the wider Project Jupyter when it works as expected.

Jupyternetes effectively breaks down the current hub down in kubernetes into multiple services and utilise the kubernetes operator design pattern and implement a more refined security policy for these services. 

## The benefits

### More Secure
By splitting out the services, refine the permissions of each service giving them only access to what they need to perform their work. This keeps the solution within principles of least privilege on kubernetes RBAC.

By keeping these services separate, it limits what damage can be done if someone were to breach each service's security. This means that if jupyterhub is breached they cannot create pods or workloads directly, they will instead have to then additionally find a way to exploit the jupyternetes operators.

#### Additional Security
Additional validation can be placed on the operator services to reduce the opportunities to use these services to exploit the solution.

### More Robust
By splitting out the services, the capabilities are also split off from one another, meaning that if there is a failure on one capability need not impact all users or templates.

### Easier to amend pods
While Kubespawner does allow a large amount of flexibility it does require you to amend code every time you want to make a simple change to a pod definition. If for example you wanted to add an environmental variable to a pod, you would need to go and amend your custom code, you'd need to test your python code then build your image and upload it to a repository. In Jupyternetes you can do the same change in seconds with a change to the template custom resource definition.

### Easier to manage templates
Adding, modifying or removing templates is very easy in Jupyternetes, you can do this using the standard **kubectl** CLI. 


#### Making it even easier
A management portal is planned and will be provided if there is enough demand within the community. 

#### Highly Extensible
The JupyterNotebookInstance Custom Resource Definition allows for other resources other than the standard PVC and Pods to be populated and also provides for statuses to be added. This means that we can easily template out new operators for a variety of purposes. As a result this can be highly extensible.
