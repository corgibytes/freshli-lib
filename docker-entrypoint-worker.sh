#!/bin/bash

set -e

export PATH="$PATH:/root/.dotnet/tools"

# build and run the website if no arguments
if [ $# -eq 0 ]; then
    # This ensures that the web project has been built correctly
    # before we start to build the worker project.
    echo 'Waiting for web container to start...'
    until $(curl --output /dev/null --silent --head --fail http://web); do
        sleep 1
    done
    echo '... web container is now running'

    echo 'Building worker'
    cd Freshli.Web.Worker
    dotnet restore --ignore-failed-sources
    dotnet build --no-restore
    
    echo 'Running worker'
    dotnet run --no-build --no-launch-profile
fi

# allows running commands in the container such as "bash"
if [ $# -gt 0 ]; then
    exec "$@"
fi
