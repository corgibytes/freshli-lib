#!/bin/bash

set -ex

export PATH="$PATH:/root/.dotnet/tools"

# build and run the website if no arguments
if [ $# -eq 0 ]; then
    dotnet restore
    dotnet build
    cd Freshli.Web
    dotnet ef database update
    dotnet run --no-build --no-launch-profile
fi

# allows running commands in the container such as "bash"
if [ $# -gt 0 ]; then
    exec "$@"
fi
