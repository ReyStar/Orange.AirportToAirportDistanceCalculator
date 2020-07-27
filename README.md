# Orange.AirportToAirportDistanceCalculator
Test project for providing information on distance between airports.

## Introduction
Description of the test task: <br>
Build a REST service to measure distance in miles between two airports. Airports are identified by 3-letter IATA code.
Sample call to get airport details: GET https://example/airports/AMS HTTP/1.1
Requirement: 
- dotnet core 2.0 or later has to be used.
Allowed to use 3-rd party components.

## Analisys
The main purpose of the development of this software is to provide functionality for calculating the distance between airports using IATA airport codes.
An IATA airport code, also known as an IATA location identifier, IATA station code, or simply a location identifier, is a **three-letter** geocode designating many airports and metropolitan areas around the world, defined by the International Air Transport Association (IATA). The characters prominently displayed on baggage tags attached at airport check-in desks are an example of a way these codes are used. [link](https://en.wikipedia.org/wiki/IATA_airport_code)
###### About performance
Before starting the development, it should be noted that the geographical coordinates of airports change very rarely and the most optimal in terms of performance is the approach of obtaining pre-calculated data (saved in the program's metadata).
### Data format
#### API

|Verb|Resource|Parameters|Status Code|Responce body|Description|
|----|--------|----------|-----------|-------------|-----------|
|GET |/api/v1/distances|from - Departure airport IATA code <br> to - Destination airport IATA code |200 - Ok <br> 400 - Bad Request <br> 404 - Not Found |DistanceResponse|Calculate distance between airports|

##### DistanceResponse
```javascript
DistanceResponse{
miles*	number($double)
}
```

### Algorithm of calculation of distances
The following three algorithms for calculating the distance by geographic coordinates are most commonly used:
1. **Vincenty's formula** is very complex and very slow, but very accurate. [link](https://en.wikipedia.org/wiki/Vincenty%27s_formulae). (The algorithm may give an inaccurate result of 0.5 mm)
2. **Haversine** simple but not very accurate value [link](https://en.wikipedia.org/wiki/Haversine_formula). The approach has problems with an accuracy of 0.5%. (from 0.42% at a distance of 300 meters, to 0.78% at a distance of 10,000 km)
3. **Spherical Law of Cosines** is a simple and fast algorithm that works over short distances. [link](https://www.movable-type.co.uk/scripts/latlong.html) There is a problem of accumulation of error from an increase in the distance from 0.27% for a distance of 100 km to 72.56% for a distance of 5000 km.

Excellent description of geo distance calculation approaches and performance improvements Haversine's algorithm using approximation and Taylor series, can see [here](https://www.youtube.com/watch?v=pTZqP78YYIA).

To implement the business logic for calculating the distance between airports, we will choose the Haversine algorithm. This algorithm is fast, reasonably accurate.


