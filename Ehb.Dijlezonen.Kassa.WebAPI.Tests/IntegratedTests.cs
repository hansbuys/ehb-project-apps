using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Tests
{
    public abstract class IntegratedTests : IDisposable
    {
        private readonly ITestOutputHelper output;
        private readonly TestServer server;

        private HttpResponseMessage LoginResponse { get; set; }

        protected IntegratedTests(ITestOutputHelper output)
        {
            this.output = output;
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
        }

        protected async Task<HttpClient> GetSut()
        {
            var client = server.CreateClient();

            if (LoginResponse != null && LoginResponse.IsSuccessStatusCode)
            {
                await SetClientCredentials(client);
            }

            return client;
        }

        public void Dispose()
        {
            server?.Dispose();
        }

        protected async Task LoginUsingUserCredentials()
        {
            output.WriteLine("Logging in as regular user");

            LoginResponse = await DoLoginUsingUserCredentials();
        }
        
        protected async Task<HttpResponseMessage> DoLoginUsingUserCredentials(string password = null)
        {
            using (var client = await GetSut())
            {
                var response = await client.PostAsync("/api/token", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", "gebruiker"),
                    new KeyValuePair<string, string>("password", password ?? "gebruiker")
                }));

                return response;
            }
        }

        protected async Task LoginUsingAdminCredentials()
        {
            output.WriteLine("Logging in as admin");

            LoginResponse = await DoLoginUsingAdminCredentials();
        }

        protected async Task<HttpResponseMessage> DoLoginUsingAdminCredentials()
        {
            using (var client = await GetSut())
            {
                var rawResponse = await client.PostAsync("/api/token", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", "beheerder"),
                    new KeyValuePair<string, string>("password", "beheerder")
                }));

                rawResponse.EnsureSuccessStatusCode();

                return rawResponse;
            }
        }

        private async Task SetClientCredentials(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetAccessToken(LoginResponse));

            output.WriteLine("client is bearing login token");
        }

        protected async Task<string> GetAccessToken(HttpResponseMessage httpResponse)
        {
            var response = await ParseJsonResponse(httpResponse);

            var accessToken = (string) response.access_token;

            output.WriteLine($"Received token: {accessToken}");

            return accessToken;
        }

        protected async Task<dynamic> ParseJsonResponse(HttpResponseMessage httpResponse)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();

            output.WriteLine($"Received web content: {responseString}");

            return JsonConvert.DeserializeObject(responseString);
        }

        protected async Task<HttpResponseMessage> PostJson(string url, object body)
        {
            using (var client = await GetSut())
            {
                var postBody = JsonConvert.SerializeObject(body);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await client.PostAsync(url, new StringContent(postBody, Encoding.UTF8, "application/json"));
            }
        }

        protected async Task<HttpResponseMessage> Get(string url)
        {
            using (var client = await GetSut())
                return await client.GetAsync(url);
        }

        protected async Task<HttpResponseMessage> PostForm(string url, IEnumerable<KeyValuePair<string, string>> form)
        {
            using (var client = await GetSut())
                return await client.PostAsync(url, new FormUrlEncodedContent(form));
        }

        protected void Logout()
        {
            LoginResponse = null;
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