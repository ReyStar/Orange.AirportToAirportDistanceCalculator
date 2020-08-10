using System.ComponentModel.DataAnnotations;
using Routes.API.Attributes;

namespace Routes.API.Controllers.V1.DTO
{
    public class RoutePoint
    {
        [Required]
        [IATACodeValidation]
        public string IATACode { get; set; }

        [Required]
        public GeoCoordinate Coordinate { get; set; }
    }
}
