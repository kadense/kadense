#!/bin/bash

dotnet new install /workspaces/kadense/c-sharp/templates/library
dotnet new install /workspaces/kadense/c-sharp/templates/kadense-operator
minikube start