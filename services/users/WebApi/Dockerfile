# Use the official ASP.NET 8 base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build and publish the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY "./Api/" "Api"
COPY "./WebApi/" "WebApi"

RUN dotnet restore "WebApi/WebApi.csproj"
RUN dotnet build "WebApi/WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi/WebApi.csproj" -c Release -o /app/publish

# Finalize the image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]