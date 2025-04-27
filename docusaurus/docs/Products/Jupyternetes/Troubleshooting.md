---
sidebar_position: 6
title: Troubleshooting
---

When a user logs into JupyterHub the Jupyternetes Spawner should create a **JupyterNotebookInstance** resource in kubernetes based upon the user that's just logged in as a result checking the logs for jupyterhub is usually the best place to start:

```bash
kubectl logs deployments/hub -n jupyterhub
```

This should give a response similar to the following:
```text
[I 2025-04-24 14:50:00.272 JupyterHub app:3346] Running JupyterHub version 5.2.1
[I 2025-04-24 14:50:00.272 JupyterHub app:3376] Using Authenticator: jupyterhub.auth.DummyAuthenticator-5.2.1
[I 2025-04-24 14:50:00.272 JupyterHub app:3376] Using Spawner: jupyternetes_spawner.spawner.JupyternetesSpawner
[I 2025-04-24 14:50:00.272 JupyterHub app:3376] Using Proxy: jupyterhub.proxy.ConfigurableHTTPProxy-5.2.1
/usr/local/lib/python3.12/site-packages/jupyter_events/schema.py:68: JupyterEventsVersionWarning: The `version` property of an event schema must be a string. It has been type coerced, but in a future version of this library, it will fail to validate. Please update schema: https://schema.jupyter.org/jupyterhub/events/server-action
  validate_schema(_schema)
[W 2025-04-24 14:50:00.449 JupyterHub auth:1508] Using testing authenticator DummyAuthenticator! This is not meant for production!
[I 2025-04-24 14:50:00.493 JupyterHub app:2919] Creating service jupyterhub-idle-culler without oauth.
[I 2025-04-24 14:50:00.608 JupyterHub app:3416] Initialized 0 spawners in 0.009 seconds
[I 2025-04-24 14:50:00.614 JupyterHub metrics:373] Found 1 active users in the last ActiveUserPeriods.twenty_four_hours
[I 2025-04-24 14:50:00.615 JupyterHub metrics:373] Found 1 active users in the last ActiveUserPeriods.seven_days
[I 2025-04-24 14:50:00.615 JupyterHub metrics:373] Found 1 active users in the last ActiveUserPeriods.thirty_days
[I 2025-04-24 14:50:00.616 JupyterHub app:3703] Not starting proxy
[I 2025-04-24 14:50:00.620 JupyterHub app:3739] Hub API listening on http://:8081/hub/
[I 2025-04-24 14:50:00.620 JupyterHub app:3741] Private Hub API connect url http://hub:8081/hub/
[I 2025-04-24 14:50:00.620 JupyterHub app:3615] Starting managed service jupyterhub-idle-culler
[I 2025-04-24 14:50:00.620 JupyterHub service:423] Starting service 'jupyterhub-idle-culler': ['python3', '-m', 'jupyterhub_idle_culler', '--url=http://localhost:8081/hub/api', '--timeout=3600', '--cull-every=600', '--concurrency=10']
[I 2025-04-24 14:50:00.624 JupyterHub service:136] Spawning python3 -m jupyterhub_idle_culler --url=http://localhost:8081/hub/api --timeout=3600 --cull-every=600 --concurrency=10
[I 2025-04-24 14:50:00.629 JupyterHub app:3772] JupyterHub is now running, internal Hub API at http://hub:8081/hub/
[I 2025-04-24 14:50:00.937 JupyterHub log:192] 200 GET /hub/api/ (jupyterhub-idle-culler@::1) 14.66ms
[I 2025-04-24 14:50:00.974 JupyterHub log:192] 200 GET /hub/api/users?state=[secret] (jupyterhub-idle-culler@::1) 34.92ms
[I 2025-04-24 14:50:11.984 JupyterHub log:192] 302 GET /hub/ -> /hub/spawn (jovyan@127.0.0.1) 20.42ms
[I 2025-04-24 14:50:12.052 JupyterHub provider:661] Creating oauth client jupyterhub-user-jovyan
[I 2025-04-24 14:50:12.080 JupyterHub utils:153] Checking if 8dc366d8a0f05869bfdb6e7eb3d83f65 in default currently exists
[I 2025-04-24 14:50:12.084 JupyterHub log:192] 302 GET /hub/spawn -> /hub/spawn-pending/jovyan (jovyan@127.0.0.1) 75.10ms
[I 2025-04-24 14:50:12.105 JupyterHub utils:156] Creating instance 8dc366d8a0f05869bfdb6e7eb3d83f65 in default
[I 2025-04-24 14:50:12.197 JupyterHub pages:397] jovyan is pending spawn
[I 2025-04-24 14:50:12.305 JupyterHub log:192] 200 GET /hub/spawn-pending/jovyan (jovyan@127.0.0.1) 173.22ms
[I 2025-04-24 14:50:22.869 JupyterHub log:192] 200 GET /hub/api (@10.244.1.126) 1.14ms
[I 2025-04-24 14:50:23.218 JupyterHub log:192] 200 POST /hub/api/users/jovyan/activity (jovyan@10.244.1.126) 36.77ms
[I 2025-04-24 14:50:23.562 JupyterHub base:1124] User jovyan took 11.548 seconds to start
[I 2025-04-24 14:50:23.562 JupyterHub proxy:331] Adding user jovyan to proxy /user/jovyan/ => http://10.244.1.126:8888
[I 2025-04-24 14:50:23.568 JupyterHub users:899] Server jovyan is ready
```

You can see the following line above:
```text
[I 2025-04-24 14:50:12.105 JupyterHub utils:156] Creating instance 8dc366d8a0f05869bfdb6e7eb3d83f65 in default
```
This indicates that the instance being created will be called **8dc366d8a0f05869bfdb6e7eb3d83f65** in the **default** namespace

## Another way to find the JupyterNotebookInstance
If you cannot access the JupyterHub logs as above to find the JupyterNotebookInstance as above then you can find this using kubectl as follows. 

When the Jupyternetes Spawner creates the JupyterNotebookInstance it creates a unique name for the resource based upon the username.

```bash
kubectl get JupyterNotebookInstance -A -o yaml
```

or you can use the shorthand:

```bash
kubectl get jni -A -o yaml
```

This should give you an output as follows:

```yaml
apiVersion: v1
items:
- apiVersion: kadense.io/v1
  kind: JupyterNotebookInstance
  metadata:
    creationTimestamp: "2025-04-27T11:28:24Z"
    generation: 1
    name: 8dc366d8a0f05869bfdb6e7eb3d83f65
    namespace: default
    resourceVersion: "401205"
    uid: 4cb17369-31bb-440d-87b6-f2e81ce8b0d2
  spec:
    template:
      name: default-template
      namespace: kadense
    variables:
      jupyterhub.api_token: 885c3c38068b4efb9a5cafb026283cc8
      jupyterhub.namespace: kadense
      jupyterhub.oauth_client_id: jupyterhub-user-jovyan
      jupyterhub.user.id: "1"
      jupyterhub.user.name: jovyan
      jupyternetes.instance.name: 8dc366d8a0f05869bfdb6e7eb3d83f65
      jupyternetes.instance.namespace: default
kind: List
metadata:
  resourceVersion: ""
```

As you can see the username / user id is present in the variables list.


## Reviewing the JupyterNotebookInstance
Once you've identified the **JupyterNotebookInstance** you can review that to ensure it is being populated correctly:

```bash
kubectl get -n default jni/8dc366d8a0f05869bfdb6e7eb3d83f65 -o yaml
```

This should provide output as follows:

```text
apiVersion: kadense.io/v1
kind: JupyterNotebookInstance
metadata:
  creationTimestamp: "2025-04-27T11:28:24Z"
  generation: 1
  name: 8dc366d8a0f05869bfdb6e7eb3d83f65
  namespace: default
  resourceVersion: "401272"
  uid: 4cb17369-31bb-440d-87b6-f2e81ce8b0d2
spec:
  template:
    name: default-template
    namespace: kadense
  variables:
    jupyterhub.api_token: 885c3c38068b4efb9a5cafb026283cc8
    jupyterhub.namespace: kadense
    jupyterhub.oauth_client_id: jupyterhub-user-jovyan
    jupyterhub.user.id: "1"
    jupyterhub.user.name: jovyan
    jupyternetes.instance.name: 8dc366d8a0f05869bfdb6e7eb3d83f65
    jupyternetes.instance.namespace: default
status:
  otherResources: {}
  otherResourcesProvisioned: {}
  pods:
    notebook:
      errorMessage: ""
      parameters: {}
      podAddress: 10.244.1.131
      resourceName: notebook-nvpzz
      state: Running
  podsProvisioned: Completed
  pvcs:
    workspace:
      parameters: {}
      resourceName: workspace-fphcr
      state: Processed
  pvcsProvisioned: Completed
```

The key thing to investigate here is the status field. if **pvcsProvisioned** is **Completed** then it indicates that the PVC operator has completed. If not this could indicate a problem with the PVC operator, or the PVC template. and therefore it may be best to check the [PVC operator logs](./Operators/pvc-operator/troubleshooting.md).

If **podsProvisioned** is **Completed** then it indicates that the Pod Operator has completed. If not this could indicate a problem with the Pods Operator,  or the pod template and therefore it may be best to check the [Pod operator logs](./Operators/pod-operator/troubleshooting.md).

If both are completed then it is likely that the pods and pvcs have been created successfully but for whatever reason the pod hasn't started and therefore it may be worth checking the pod events and logs.

## Checking for events against the instance
Once you've identified the **JupyterNotebookInstance** you can then query events to see the events relating to that instance:

```bash
kubectl get -n default events --field-selector involvedObject.name=8dc366d8a0f05869bfdb6e7eb3d83f65,involvedObject.kind=JupyterNotebookInstance --sort-by eventTime
```

This should provide an output as follows:

```text
LAST SEEN   TYPE     REASON              OBJECT                                                     MESSAGE
25m         Normal   Resource Created    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   PodAddressUpdated   jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   Pod notebook address updated to 10.244.1.131
25m         Normal   PodIsRunning        jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   Pod state on pod notebook is updated to Running
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
25m         Normal   Resource Updated    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
9m51s       Normal   Resource Created    jupyternotebookinstance/8dc366d8a0f05869bfdb6e7eb3d83f65   
```

As you can see this gives you more information about the instance and what has happened on it, you can deep dive into this by exporting the values to yaml or json.

```bash
kubectl get -n default events --field-selector involvedObject.name=8dc366d8a0f05869bfdb6e7eb3d83f65,involvedObject.kind=JupyterNotebookInstance --sort-by eventTime -o json | jq .items[].related
```

This should give you a list of the related objects:

```text
null
null
null
null
null
null
null
null
null
{
  "apiVersion": "v1",
  "kind": "Pod",
  "name": "notebook-nvpzz",
  "namespace": "default",
  "resourceVersion": "401263",
  "uid": "56dde094-f587-49b3-9af0-46aceb92db3a"
}
{
  "apiVersion": "v1",
  "kind": "Pod",
  "name": "notebook-nvpzz",
  "namespace": "default",
  "resourceVersion": "401263",
  "uid": "56dde094-f587-49b3-9af0-46aceb92db3a"
}
null
null
null
null
```

From this you can see that the pod relating to this is notebook-nvpzz

