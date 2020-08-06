#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Services/AirportInfo/AirportInfo.Shell/AirportInfo.Shell.csproj", "src/Services/AirportInfo/AirportInfo.Shell/"]
RUN dotnet restore "src/Services/AirportInfo/AirportInfo.Shell/AirportInfo.Shell.csproj"
COPY . .
WORKDIR "/src/src/Services/AirportInfo/AirportInfo.Shell"
RUN dotnet build "AirportInfo.Shell.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AirportInfo.Shell.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AirportInfo.Shell.dll"]