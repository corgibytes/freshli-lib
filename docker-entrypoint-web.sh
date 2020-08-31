#!/bin/bash

set -ex

export PATH="$PATH:/root/.dotnet/tools"

# build and run the website if no arguments
if [ $# -eq 0 ]; then
    cd XPlot
    dotnet tool restore
    dotnet paket restore
    cd -
    cd Freshli.Web
    dotnet restore
    dotnet build --no-restore
    dotnet ef database update --context ApplicationDbContext
    dotnet ef database update --context ApplicationIdentityDbContext
    dotnet run --no-build --no-launch-profile
fi

# allows running commands in the container such as "bash"
if [ $# -gt 0 ]; then
    exec "$@"
fi
