FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 5002

# Copy everything
COPY ./src/Modules/AntCommerce.Module.Core ./AntCommerce.Module.Core
COPY ./src/Modules/AntCommerce.Module.Message ./AntCommerce.Module.Message
COPY ./src/Modules/AntCommerce.Module.Web ./AntCommerce.Module.Web
COPY ./src/Modules/AntCommerce.Module.Product ./AntCommerce.Module.Product

# Build and publish a release
RUN dotnet restore AntCommerce.Module.Product/AntCommerce.Module.Product.csproj

RUN dotnet publish AntCommerce.Module.Product/AntCommerce.Module.Product.csproj -c Release -o out --no-restore -p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "AntCommerce.Module.Product.dll"]