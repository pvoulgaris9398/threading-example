FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /ThreadingWithDocker

# Copy everything
COPY . ./

VOLUME ["/var/log/"]

# Restore as distinct layers
RUN dotnet restore

# Build & publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /ThreadingWithDocker
COPY --from=build-env /ThreadingWithDocker/out .
ENTRYPOINT ["dotnet", "ThreadingWithDocker.dll"]



