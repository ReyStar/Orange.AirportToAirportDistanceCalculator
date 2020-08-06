using System;

namespace DistanceCalculator.Repository.Models
{
    /// <summary>
    /// Version model for migration table
    /// </summary>
    class VersionInfoDbModel
    {
        public int Version { get; set; }
        
        public DateTime? AppliedOn { get; set; }
        
        public string Description { get; set; }
    }
}
