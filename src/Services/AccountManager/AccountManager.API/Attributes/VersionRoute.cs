using Microsoft.AspNetCore.Mvc;

namespace AccountManager.API.Attributes
{
    public class VersionRoute : RouteAttribute
    {
        public VersionRoute(string prefix) : base($"api/v{{version:apiVersion}}/{prefix}")
        {
        }
    }
}
