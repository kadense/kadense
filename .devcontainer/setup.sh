#!/bin/bash

dotnet new install /workspaces/kadense/c-sharp/templates/library
dotnet new install /workspaces/kadense/c-sharp/templates/kadense-operator
minikube start
minikube addons enable registry
kubectl port-forward --namespace kube-system service/registry 5000:80 &