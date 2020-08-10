using System.ComponentModel.DataAnnotations;

namespace Routes.API.Controllers.V1.DTO
{
    public class GeoCoordinate
    {
        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Latitude { get; set; }
    }
}
