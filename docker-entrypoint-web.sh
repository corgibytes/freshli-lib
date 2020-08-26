#!/bin/bash

set -ex

export PATH="$PATH:/root/.dotnet/tools"

export DB_HOST=${DB_HOST:-localhost}
export DB_USERNAME=${DB_USERNAME:-postgres}
export DB_PASSWORD=${DB_PASSWORD:-password}
export DB_NAME=${DB_NAME:-freshli_web_development}

# build and run the website if no arguments
if [ $# -eq 0 ]; then
    if [ $ASPNETCORE_ENVIRONMENT != "Production" ]; then
        cd XPlot
        dotnet tool restore
        dotnet paket restore
        cd -
        cd Freshli.Web
        dotnet restore
        dotnet build --no-restore
        dotnet ef database update
        dotnet run --no-build --no-launch-profile
    else
        PGPASSWORD=$DB_PASSWORD psql --host=$DB_HOST --username=$DB_USERNAME --dbname=$DB_NAME < migrations.sql
        ./Freshli.Web
    fi
fi

# allows running commands in the container such as "bash"
if [ $# -gt 0 ]; then
    exec "$@"
fi
