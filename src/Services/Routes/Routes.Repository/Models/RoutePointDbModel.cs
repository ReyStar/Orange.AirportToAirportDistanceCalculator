using System;
using System.Collections.Generic;
using System.Text;

namespace Routes.Repository.Models
{
    class RoutePointDbModel
    {
        public string IATACode { get; set; }

        public GeoCoordinateDbModel Coordinate { get; set; }
    }
}
