# syntax=docker/dockerfile:1.2

ARG VARIANT=6.0
FROM  mcr.microsoft.com/dotnet/sdk:${VARIANT}
RUN apt-get update && apt-get install -y zip

WORKDIR /freshli
COPY . .
