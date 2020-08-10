using Microsoft.AspNetCore.Mvc;

namespace Routes.API.Attributes
{
    /// <summary>
    /// Api version attribute, use only definition api versions
    /// </summary>
    public class WebApiVersionAttribute : ApiVersionAttribute
    {
        public WebApiVersionAttribute(ApiVersions version) 
            : base(new ApiVersion((int) version, 0))
        {
        }
    }
}
