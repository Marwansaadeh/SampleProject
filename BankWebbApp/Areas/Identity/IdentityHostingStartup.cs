using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BankWebbApp.Areas.Identity.IdentityHostingStartup))]
namespace BankWebbApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}