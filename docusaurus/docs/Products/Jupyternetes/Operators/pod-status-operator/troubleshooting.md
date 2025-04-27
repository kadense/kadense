---
title: Troubleshooting
sidebar_position: 3
---

To load the logs of the Pod Status Operator you can use kubectl to query the container logs:

```bash
kubectl logs deployments/jupyternetes-pods-op -n kadense -c pod-status-operator
```

