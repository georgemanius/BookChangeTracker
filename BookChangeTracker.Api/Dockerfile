FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY BookChangeTracker.sln ./
COPY BookChangeTracker.Api/BookChangeTracker.Api.csproj BookChangeTracker.Api/
COPY BookChangeTracker.Application/BookChangeTracker.Application.csproj BookChangeTracker.Application/
COPY BookChangeTracker.Domain/BookChangeTracker.Domain.csproj BookChangeTracker.Domain/
COPY BookChangeTracker.Infrastructure/BookChangeTracker.Infrastructure.csproj BookChangeTracker.Infrastructure/

RUN dotnet restore BookChangeTracker.Api/BookChangeTracker.Api.csproj

COPY . .
WORKDIR /src/BookChangeTracker.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "BookChangeTracker.Api.dll"]
