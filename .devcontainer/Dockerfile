FROM mcr.microsoft.com/devcontainers/dotnet:8.0

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    yq \
    && rm -rf /var/lib/apt/lists/*