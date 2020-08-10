using Microsoft.AspNetCore.Mvc;

namespace Routes.API.Attributes
{
    public class VersionRoute : RouteAttribute
    {
        public VersionRoute(string prefix) : base($"api/v{{version:apiVersion}}/{prefix}")
        {
        }
    }
}
