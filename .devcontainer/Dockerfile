FROM mcr.microsoft.com/devcontainers/dotnet:8.0

RUN apt-get update \
    && apt-get install -y \
        podman \
    && rm -rf /var/lib/apt/lists/*