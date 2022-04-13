#!/bin/bash

set -e

[ -e files ] || ln -s ../../resources files

if ! type "nuget" > /dev/null 2>&1; then
  echo 'Please install nuget:'
  echo 'https://docs.nuget.org/ndocs/guides/install-nuget'
  exit 1
fi

if ! type "xbuild" > /dev/null 2>&1; then
  echo 'Please install Xamarin studio:'
  echo 'https://www.xamarin.com/download-it'
  exit 1
fi

nuget install -OutputDirectory packages
