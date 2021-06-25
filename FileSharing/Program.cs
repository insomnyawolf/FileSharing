using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FileSharing
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.CaptureStartupErrors(false);
                     webBuilder.UseStartup<Startup>();
                 });

            builder.Build().Run();
        }
    }
}
