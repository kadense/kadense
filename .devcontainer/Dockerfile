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

RUN curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg \
    && mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg \
    && sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/debian/$(lsb_release -rs 2>/dev/null | cut -d'.' -f 1)/prod $(lsb_release -cs 2>/dev/null) main" > /etc/apt/sources.list.d/dotnetdev.list' \
    && apt-get update \
    && apt-get install azure-functions-core-tools-4 -y


COPY ./install-rabbit-mq.sh /tmp/install-rabbit-mq.sh
RUN /tmp/install-rabbit-mq.sh
