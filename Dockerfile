# Build frontend
FROM node:buster as build-frontend
WORKDIR /build

COPY ./Frontend/package*.json ./
RUN npm install

COPY ./Frontend /build
RUN npm run build


# Build backend
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-backend
WORKDIR /build

COPY ./Backend/*.csproj ./
RUN dotnet restore

COPY ./Backend/* ./
RUN dotnet publish -c Release -o out


# Finally launch
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-backend /build/out .
COPY --from=build-frontend /build/dist ./wwwroot
ENTRYPOINT [ "dotnet", "AuthServer.dll" ]