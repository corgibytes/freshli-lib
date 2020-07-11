#!/bin/bash

# build and run the website if no arguments
if [ $# -eq 0 ]; then
    dotnet restore
    dotnet build
    dotnet run --project=Freshli.Web/Freshli.Web.csproj --no-build --no-launch-profile
fi

# allows running commands in the container such as "bash"
if [ $# -gt 0 ]; then
    exec "$@"
fi
