using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SecureNotesWebClient.Areas.Identity.IdentityHostingStartup))]
namespace SecureNotesWebClient.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}