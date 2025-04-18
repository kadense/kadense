ARG DOTNET_SDK_VERSION=8.0
ARG PYTHON_VERSION=3.13-alpine
ARG KADENSE_VERSION="0.1.0"
ARG KADENSE_VERSION_SUFFIX="-local"
ARG JUPYTERHUB_BASE_IMAGE="quay.io/jupyterhub/k8s-hub:4.1.0"

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK_VERSION} AS builder
ARG KADENSE_VERSION
ARG KADENSE_VERSION_SUFFIX
ARG DOTNET_SDK_VERSION
COPY ./c-sharp /workspaces/kadense/c-sharp
WORKDIR /workspaces/kadense/c-sharp
RUN dotnet build
RUN dotnet publish -c Release /p:Version=${KADENSE_VERSION}${KADENSE_VERSION_SUFFIX} /p:AssemblyVersion=${KADENSE_VERSION}
RUN mkdir -p /outputs/crds && \
    dotnet /workspaces/kadense/c-sharp/cli/CustomResourceDefinition.Generator/src/bin/Release/net${DOTNET_SDK_VERSION}/publish/Kadense.CustomResourceDefinition.Generator.dll /outputs/crds

FROM scratch AS crds-artifact
COPY --from=builder "/outputs/crds" "/outputs"

FROM mcr.microsoft.com/dotnet/runtime:${DOTNET_SDK_VERSION} AS jupyternetes-pods-operator
ARG DOTNET_SDK_VERSION
COPY --from=builder "/workspaces/kadense/c-sharp/operators/Jupyternetes.Pods.Operator/src/bin/Release/net${DOTNET_SDK_VERSION}/publish/" "/app"
WORKDIR /app
ENTRYPOINT ["dotnet", "Kadense.Jupyternetes.Pods.Operator.dll"]
USER 999

FROM mcr.microsoft.com/dotnet/runtime:${DOTNET_SDK_VERSION} AS jupyternetes-pvcs-operator
ARG DOTNET_SDK_VERSION
COPY --from=builder "/workspaces/kadense/c-sharp/operators/Jupyternetes.Pvcs.Operator/src/bin/Release/net${DOTNET_SDK_VERSION}/publish/" "/app"
WORKDIR /app
ENTRYPOINT ["dotnet", "Kadense.Jupyternetes.Pvcs.Operator.dll"]
USER 999

FROM python:${PYTHON_VERSION} AS python-libraries
ARG KADENSE_VERSION
RUN pip install --no-cache-dir --upgrade \
    pip \ 
    setuptools \
    wheel \
    build \
    twine \
    pytest \
    pydantic \
    kubernetes-asyncio \
    pytz \
    jupyterhub \
    pytest-asyncio 

COPY ./python /workspaces/kadense/python
RUN export KADENSE_PY_VERSION="__version__ = \"${KADENSE_VERSION}\""; \
    echo "KADENSE_PY_VERSION=${KADENSE_PY_VERSION}"; \
    echo "${KADENSE_PY_VERSION}" > /workspaces/kadense/python/jupyternetes_models/jupyternetes_models/_version.py; \
    echo "${KADENSE_PY_VERSION}" > /workspaces/kadense/python/jupyternetes_clients/jupyternetes_clients/_version.py; \
    echo "${KADENSE_PY_VERSION}" > /workspaces/kadense/python/jupyternetes_spawner/jupyternetes_spawner/_version.py; \
    mkdir -p /tmp/dist;

WORKDIR /workspaces/kadense/python/jupyternetes_spawner
RUN python -m pytest; 
RUN python -m build; \
    mv dist/* /tmp/dist

FROM scratch AS python-libraries-artifact
COPY --from=python-libraries /tmp/dist/ /outputs

FROM ${JUPYTERHUB_BASE_IMAGE} AS jupyternetes-hub
USER root
COPY ./python/jupyternetes_spawner/ /src/jupyternetes-spawner
RUN export KADENSE_PY_VERSION="__version__ = \"${KADENSE_VERSION}\""; \
    echo "KADENSE_PY_VERSION=${KADENSE_PY_VERSION}"; \
    echo "${KADENSE_PY_VERSION}" > /src/jupyternetes-spawner/jupyternetes_spawner/_version.py; \
RUN python -mpip install /src/jupyternetes-spawner
USER jovyan