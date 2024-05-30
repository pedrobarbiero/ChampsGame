FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ChampsGame.sln ./
COPY src/ ./src/

RUN dotnet restore

RUN dotnet publish src/Champs.Client/Champs.Client.csproj -c Release -o out

FROM nginx:alpine
COPY --from=build-env app/out/wwwroot /usr/share/nginx/html
COPY src/Champs.Client/nginx.conf /etc/nginx/nginx.conf
