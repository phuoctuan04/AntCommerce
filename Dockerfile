FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
EXPOSE 5000

# Copy everything
COPY src/Modules ./Modules
COPY src/WebHost ./WebHost

# Build and publish a release
RUN dotnet restore WebHost/WebHost.csproj

RUN dotnet publish WebHost/WebHost.csproj -c Release -o out --no-restore -p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "WebHost.dll"]