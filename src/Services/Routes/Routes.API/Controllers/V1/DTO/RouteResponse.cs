using System;
using System.Collections.Generic;

namespace Routes.API.Controllers.V1.DTO
{
    public class RouteResponse
    {
        public Guid Id { get; set; }

        public IEnumerable<RoutePoint> Points { get; set; }
    }
}
