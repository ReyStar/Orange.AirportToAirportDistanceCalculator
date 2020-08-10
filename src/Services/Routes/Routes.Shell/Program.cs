using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Routes.Shell.Configuration;

namespace Routes.Shell
{
    public class Program
    {
        private const int SuccessExitCode = 0;
        private const int ErrorExitCode = 1;

        public static async Task<int> Main(string[] args)
        {
            try
            {
                //it helped  search files under debugger, without all worked normally
                Directory.SetCurrentDirectory(PlatformServices.Default.Application.ApplicationBasePath);

                await CreateHostBuilder(args).Build()
                                             .RunAsync();
                return SuccessExitCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ErrorExitCode;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })

                    .RegisterSerilog();
        }       
    }
}