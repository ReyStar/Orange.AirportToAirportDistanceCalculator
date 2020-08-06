namespace DistanceCalculator.Repository.Infrastructure
{
    public class ConnectionStrings
    {
        /// <summary>
        /// Database connection string
        /// </summary>
        public string DefaultConnection { get; set; }

        /// <summary>
        /// Required database version
        /// </summary>
        public int RequiredVersion { get; set; }
    }
}
