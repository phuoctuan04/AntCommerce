# Ant Commerce
The simple commerce with .NET Core.

### Build status
[![Build](https://github.com/tuanitpro/AntCommerce/actions/workflows/build.yml/badge.svg)](https://github.com/tuanitpro/AntCommerce/actions/workflows/build.yml)

# High level architecture

![Swagger UI](docs/images/modular-architecture.png)

# How to work

## Prerequisites
- .NET Core 6
- Visual Studio 2022
- SQL Server

## In this repository
- .NET Core
- Entity Framework
- Restful API
- SQL Server
- Swagger
- Serilog / Logging
- AutoMapper
- MediatR
- FluentValidation
- CQRS
- Unit tests (xUnit, NSubstitute, MockQueryable)
- Dockerfile

## Steps to run
Using Docker
> docker-compose up

Init Database
> docker exec -it  sqlserver  bash  /opt/script/init.sh

Using git clone

> git@github.com:tuanitpro/AntCommerce.git

> run sql/init-script to create table & data

> dotnet restore

> cd WebHost > dotnet run

Open browser then Enter 
> http://localhost:5001

## Demo
> https://product-api.tuanitpro.com
# Output result

![Swagger UI](docs/images/Screenshot_4.png)


![Unit Test coverage](docs/images/Screenshot_5.png)


# References
![Rest Design](docs/images/rest-design.png)
> https://blog.devgenius.io/best-practice-and-cheat-sheet-for-rest-api-design-6a6e12dfa89f
