#!/bin/bash

# Script to create Entity Framework migrations using Docker

echo "Creating Entity Framework migration..."

# Run dotnet ef migrations add in a temporary container
docker run --rm \
  -v "$(pwd)/API:/src" \
  -w /src \
  mcr.microsoft.com/dotnet/sdk:8.0 \
  bash -c "dotnet tool install --global dotnet-ef --version 8.* && \
           export PATH=\"\$PATH:/root/.dotnet/tools\" && \
           dotnet ef migrations add InitialCreate"

echo "Migration created successfully!"
echo "You can now run 'docker-compose up' and migrations will be applied automatically."
