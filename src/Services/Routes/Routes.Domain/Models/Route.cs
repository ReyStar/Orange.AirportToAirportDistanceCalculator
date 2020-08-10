using System;
using System.Collections.Generic;

namespace Routes.Domain.Models
{
    /// <summary>
    /// Airport information
    /// </summary>
    public class Route
    {
        /// <summary>
        /// <summary>Initializes a new instance of the <see cref="Route" /> struct.</summary>
        /// </summary>
        public Route()
        {
        }

        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }

        public ICollection<RoutePoint> Points { get; set; }
    }
}
