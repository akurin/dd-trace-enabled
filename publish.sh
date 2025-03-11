#!/bin/bash

set -euo pipefail

# Publish LambdaNet8 project and package it into a zip file
dotnet publish src/EchoFunction --configuration Release --framework net8.0 --runtime linux-x64
zip -j EchoFunction.zip src/EchoFunction/bin/Release/net8.0/linux-x64/publish/*

