using Microsoft.AspNetCore.Mvc;

namespace AirportInfo.API.Attributes
{
    public class VersionRoute : RouteAttribute
    {
        public VersionRoute(string prefix) : base($"api/v{{version:apiVersion}}/{prefix}")
        {
        }
    }
}
