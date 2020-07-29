#docker build --no-cache -f Dockerfile . --tag atadc:latest
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env 
COPY . .
RUN dotnet restore
WORKDIR "/src/Orange.AirportToAirportDistanceCalculator.Shell"
RUN dotnet build "Orange.AirportToAirportDistanceCalculator.Shell.csproj" --runtime linux-x64 -c Debug --no-restore 

#FROM build AS publish
RUN dotnet publish -c Debug --self-contained -r linux-x64 "Orange.AirportToAirportDistanceCalculator.Shell.csproj" -o /Public --no-restore

WORKDIR /Public
#VOLUME  /Public

ENV ASPNETCORE_ENVIRONMENT Development
EXPOSE 5000 5001

CMD /Public/Orange.AirportToAirportDistanceCalculator.Shell