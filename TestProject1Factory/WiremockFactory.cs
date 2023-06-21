using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.Server;

namespace TestProject1Factory
{
    public class WiremockFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var wiremockServer = WireMockServer.Start();

            builder.ConfigureAppConfiguration(b =>
            {
                b.AddInMemoryCollection(new KeyValuePair<string, string>[] {

                new("url", wiremockServer.Urls[0])
                });
            })
                .ConfigureServices(c => c.AddSingleton(wiremockServer));
        }
    }
}
