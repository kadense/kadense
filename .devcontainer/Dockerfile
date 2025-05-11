FROM mcr.microsoft.com/devcontainers/dotnet:8.0

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    yq \
    gnupg \
    apt-transport-https \
    python3 \
    python3-pip \
    python3.11-venv \
    python3-setuptools \
    python3-wheel \
    python3-pytest \
    python3-pydantic \
    python3-kubernetes \
    && rm -rf /var/lib/apt/lists/*

COPY ./install-rabbit-mq.sh /tmp/install-rabbit-mq.sh
RUN /tmp/install-rabbit-mq.sh
