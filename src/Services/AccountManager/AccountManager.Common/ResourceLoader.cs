using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AccountManager.Common
{
    public class ResourceLoader
    {
        private readonly Assembly _assembly;

        public ResourceLoader(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentException($"The {nameof(assembly)} can not be null");
            }

            _assembly = assembly;
        }

        public async Task<string> LoadStringAsync(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentException($"The {nameof(resourceName)} can not be null or empty");
            }

            await using var resourceStream = _assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(resourceStream ?? throw new InvalidOperationException($"The {nameof(resourceStream)} can't be loaded"));
            
            return await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        public string LoadString(string scriptName)
        {
            return LoadStringAsync(scriptName).GetAwaiter().GetResult();
        }
    }
}
