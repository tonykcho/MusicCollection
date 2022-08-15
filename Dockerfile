#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY MusicCollection.sln .
COPY ["MusicCollection.API", "MusicCollection.API/"]
COPY ["MusicCollection.Application", "MusicCollection.Application/"]
COPY ["MusicCollection.DataAccess", "MusicCollection.DataAccess/"]
COPY ["MusicCollection.Domain", "MusicCollection.Domain/"]

RUN dotnet restore "MusicCollection.API/MusicCollection.API.csproj"
WORKDIR "/src/MusicCollection.API/"
RUN dotnet build "MusicCollection.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MusicCollection.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicCollection.API.dll"]