# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get -y install sudo && apt-get -y install curl

RUN curl -sL https://deb.nodesource.com/setup_22.x | sudo -E bash -
RUN apt-get install -y nodejs

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
RUN apt-get update && apt-get -y install sudo && apt-get -y install curl

RUN curl -sL https://deb.nodesource.com/setup_22.x | sudo -E bash -
RUN apt-get install -y nodejs

WORKDIR /src
COPY ["gremlin-eye.Server.csproj", "gremlin-eye.Server/"]
RUN dotnet restore "gremlin-eye.Server/gremlin-eye.Server.csproj"
COPY . .
WORKDIR "/src/gremlin-eye.Server"
RUN dotnet build "gremlin-eye.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "gremlin-eye.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gremlin-eye.Server.dll"]