#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Orange.BetweenAirportsDistanceCalculator.Shell/Orange.BetweenAirportsDistanceCalculator.Shell.csproj", "src/Orange.BetweenAirportsDistanceCalculator.Shell/"]
RUN dotnet restore "src/Orange.BetweenAirportsDistanceCalculator.Shell/Orange.BetweenAirportsDistanceCalculator.Shell.csproj"
COPY . .
WORKDIR "/src/src/Orange.BetweenAirportsDistanceCalculator.Shell"
RUN dotnet build "Orange.BetweenAirportsDistanceCalculator.Shell.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orange.BetweenAirportsDistanceCalculator.Shell.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orange.BetweenAirportsDistanceCalculator.Shell.dll"]