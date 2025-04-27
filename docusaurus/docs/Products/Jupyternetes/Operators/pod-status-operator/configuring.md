---
title: Configuring
sidebar_position: 2
---


The Pod Status Operator can be configured via the helm chart for kadense

```yaml
jupyternetes:
  pod:
    status:
      operator:
        image:
          repository: ""
          name: jupyternetes-podstatus-operator
          tag: ""
          pullPolicy: Always
        resources:
          limits:
            cpu: 100m
            memory: 128Mi
          requests:
            cpu: 100m
            memory: 128Mi
```

Once you've created your config file you can reapply the helm chart with the new configuration.