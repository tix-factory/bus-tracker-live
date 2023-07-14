# Setup runtime images
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Setup build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Use California timezone, because the buses being tracked are in California.
# Set this environment variable so the tests know that
ENV TZ="America/Los_Angeles"

# Copy Files
COPY NuGet.config /all/
COPY Directory.Build.props /all/
COPY Directory.Build.targets /all/
COPY Directory.Packages.props /all/

# Copy in only the csproj files for the restore step, to enable higher docker cache hit rate.
COPY *.sln /all/services/api/
COPY src/*.csproj /all/services/api/src/
COPY tests/*.csproj /all/services/api/tests/
WORKDIR /all/services/api

# NuGet Restore
RUN dotnet restore

# Build
COPY src src
COPY tests tests
RUN dotnet build --no-restore -c Release

# Test
RUN dotnet test --filter TestCategory=Unit

# Publish
FROM build AS publish
WORKDIR /all/services/api/src
RUN dotnet publish --no-restore --no-build -c Release --output /app

# Build runtime image
FROM base as final
WORKDIR /app
COPY --from=publish /app/ .

ENTRYPOINT ["dotnet", "TixFactory.BusTracker.Api.dll"]

ENV APP_NAME="bus-tracker-api"

# Use California timezone, because the buses being tracked are in California.
# Set this environment variable so the application knows what timezone
# to use at runtime.
ENV TZ="America/Los_Angeles"
