#!/bin/bash
set -e

export GOOGLE_APPLICATION_CREDENTIALS=$PWD/files/application_credentials.json
xbuild
mono packages/NUnit.ConsoleRunner.3.7.0/tools/nunit3-console.exe bin/Debug/dotnet.dll
