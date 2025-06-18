ARG DOTNET_SDK_VERSION=8.0
ARG PYTHON_VERSION=3.13-alpine
ARG KADENSE_VERSION="0.1.0"
ARG KADENSE_VERSION_SUFFIX="-local"
ARG JUPYTERHUB_BASE_IMAGE="quay.io/jupyterhub/k8s-hub:4.1.0"

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK_VERSION} AS builder
ARG KADENSE_VERSION
ARG KADENSE_VERSION_SUFFIX
ARG DOTNET_SDK_VERSION
COPY ./LICENSE.md /workspaces/kadense/LICENSE.md
COPY ./c-sharp /workspaces/kadense/c-sharp
WORKDIR /workspaces/kadense/c-sharp
RUN mkdir -p /outputs/nuget && \
    dotnet build -c Release /p:Version=${KADENSE_VERSION}${KADENSE_VERSION_SUFFIX} /p:AssemblyVersion=${KADENSE_VERSION} /p:PackageOutputPath=/outputs/nuget
RUN mkdir -p /root/.kube && \
    touch /root/.kube/config
RUN dotnet test 
RUN dotnet publish -c Release /p:Version=${KADENSE_VERSION}${KADENSE_VERSION_SUFFIX} /p:AssemblyVersion=${KADENSE_VERSION}

FROM scratch AS nuget-artifact
COPY --from=builder "/outputs/nuget" "/outputs"

FROM node:lts AS docs
ARG KADENSE_VERSION
ENV FORCE_COLOR=0
RUN corepack enable
COPY ./docusaurus/package.json /opt/docusaurus/package.json
COPY ./docusaurus/package-lock.json /opt/docusaurus/package-lock.json
WORKDIR /opt/docusaurus
RUN npm ci
COPY ./docusaurus/*.ts /opt/docusaurus/
COPY ./docusaurus/tsconfig.json /opt/docusaurus/
COPY ./docusaurus/blog /opt/docusaurus/blog/
COPY ./docusaurus/docs /opt/docusaurus/docs/
COPY ./docusaurus/src /opt/docusaurus/src/
COPY ./docusaurus/static /opt/docusaurus/static/

FROM docs AS docs-build
RUN npm run build
RUN chmod -R 0777 /opt/docusaurus/build/*
RUN ls -lRa /opt/docusaurus/build

FROM docs AS docs-dev
EXPOSE 3000
CMD [ "npm", "run", "start", "--", "--host", "0.0.0.0" ]

FROM scratch AS docs-artifact
COPY --from=docs-build /opt/docusaurus/build /outputs