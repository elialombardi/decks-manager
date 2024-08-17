# Use the official ASP.NET 8 base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build and publish the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY "./Api/" "Api"
COPY "./ConsoleApp/" "ConsoleApp"

RUN dotnet restore "ConsoleApp/ConsoleApp.csproj"
RUN dotnet build "ConsoleApp/ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp/ConsoleApp.csproj" -c Release -o /app/publish

# Finalize the image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp.dll"]