FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ChampsGame.sln ./
COPY src/ ./src/

RUN dotnet restore

RUN dotnet publish src/Champs.Server/Champs.Server.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Champs.Server.dll"]
