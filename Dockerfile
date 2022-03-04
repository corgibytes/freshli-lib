# syntax=docker/dockerfile:1.2

ARG VARIANT=6.0
FROM  mcr.microsoft.com/dotnet/sdk:${VARIANT}
WORKDIR /freshli
COPY . .
