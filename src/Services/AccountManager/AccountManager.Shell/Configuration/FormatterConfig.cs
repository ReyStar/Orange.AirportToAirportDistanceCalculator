using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace AccountManager.Shell.Configuration
{
    /// <summary>
    /// Media formatter config
    /// </summary>
    static class FormatterConfig
    {
        /// <summary>
        /// Registration newtonsoft json media formatter
        /// </summary>
        public static void ConfigureJsonFormat(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opt.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; 
                opt.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
        }
    }
}
