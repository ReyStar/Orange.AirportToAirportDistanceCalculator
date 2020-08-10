using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Routes.API.Controllers.V1.DTO
{
    public class RouteRequest
    {
        public Guid? Id { get; set; }

        [Required]
        public IEnumerable<RoutePoint> Points { get; set; }
    }
}
