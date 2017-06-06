using System;
using System.IO;
using System.Net.Http;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Tests
{
    public abstract class IntegratedTests : IDisposable
    {
        protected readonly HttpClient Client;
        private readonly TestServer server;

        protected IntegratedTests(ITestOutputHelper output)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            server = new TestServer(new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(logging =>
                {
                    logging.AddProvider(new XunitLoggingProvider(output, configuration.GetSection("Logging")));
                })
                .UseStartup<TestStartup>());
            Client = server.CreateClient();
        }

        public void Dispose()
        {
            server?.Dispose();
            Client?.Dispose();
        }
    }

    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
        }

        protected override void SetupDatabaseConnection(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase();
            services.AddDbContext<UserContext>((s, o) => 
                o.UseInMemoryDatabase(Guid.NewGuid().ToString()) //one DB per test instance
                 .UseInternalServiceProvider(s));
        }
    }
}