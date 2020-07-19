#!/bin/bash

set -ex

export PATH="$PATH:/root/.dotnet/tools"

# build and run the website if no arguments
if [ $# -eq 0 ]; then
    cd Freshli.Web.Worker
    dotnet restore --no-dependencies
    dotnet build --no-dependencies
    dotnet run --no-build --no-launch-profile
fi

# allows running commands in the container such as "bash"
if [ $# -gt 0 ]; then
    exec "$@"
fi
