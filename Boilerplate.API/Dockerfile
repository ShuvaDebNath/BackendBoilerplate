#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Boilerplate.API/Boilerplate.API.csproj", "Boilerplate.API/"]
COPY ["Boilerplate.Entities/Boilerplate.Entities.csproj", "Boilerplate.Entities/"]
COPY ["Boilerplate.Service/Boilerplate.Service.csproj", "Boilerplate.Service/"]
COPY ["Boilerplate.Repository/Boilerplate.Repository.csproj", "Boilerplate.Repository/"]
RUN dotnet restore "./Boilerplate.API/./Boilerplate.API.csproj"
COPY . .
WORKDIR "/src/Boilerplate.API"
RUN dotnet build "./Boilerplate.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Boilerplate.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Boilerplate.API.dll"]