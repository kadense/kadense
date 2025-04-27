---
title: Configuring
sidebar_position: 2
---


The Pod Operator can be configured via the helm chart for kadense

```yaml
jupyternetes:
  pods:
    operator:
      enabled: true
      image:
        repository: ""
        name: jupyternetes-pods-operator
        tag: ""
        pullPolicy: Always
      serviceAccount:
        create: true
        name: ""
      nodeSelector: {}
      tolerations: []
      affinity: {}
      resources:
        limits:
          cpu: 100m
          memory: 128Mi
        requests:
          cpu: 100m
          memory: 128Mi
```

Once you've created your config file you can reapply the helm chart with the new configuration.