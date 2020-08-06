using Newtonsoft.Json;

namespace AirportInfo.API.Controllers.V1.DTO
{
    public class GeoCoordinateResponse
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
