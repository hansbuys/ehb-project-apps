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
        protected UserContext Context { get; set; }

        private HttpResponseMessage LoginResponse { get; set; }

        protected IntegratedTests(ITestOutputHelper output)
        {
            this.output = output;
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(logging =>
                {
                    logging.AddProvider(new XunitLoggingProvider(output, configuration.GetSection("Logging")));
                })
                .UseStartup<TestStartup>();
            
            server = new TestServer(webHostBuilder);
        }

        protected async Task<HttpClient> GetSut()
        {
            var client = server.CreateClient();
            Context = (UserContext)server.Host.Services.GetService(typeof(UserContext));

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
            LoginResponse = await DoLoginUsingUserCredentials();
        }
        
        protected Task<HttpResponseMessage> DoLoginUsingUserCredentials(string password = null)
        {
            return DoLoginUsingCredentials("gebruiker", password ?? "gebruiker");
        }

        protected async Task<HttpResponseMessage> DoLoginUsingCredentials(string username, string password)
        {
            output.WriteLine($"Logging in as {username}");

            using (var client = await GetSut())
            {
                var response = await client.PostAsync("/api/token", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                }));

                return response;
            }
        }

        protected async Task LoginUsingAdminCredentials()
        {
            LoginResponse = await DoLoginUsingAdminCredentials();
        }

        protected Task<HttpResponseMessage> DoLoginUsingAdminCredentials()
        {
            return DoLoginUsingCredentials("beheerder", "beheerder");
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
            output.WriteLine($"Sending post to '{url}', content: {body}");

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

        protected async Task<HttpResponseMessage> CreateNewUser(string username = "john.doe@domain-name.tld", string password = "will-be-encrypted", 
            bool passwordNeedsReset = true, string firstname = "John", string lastname = "Doe")
        {
            var response = await PostJson("/api/user/create", new
            {
                Username = username,
                Password = password,
                Firstname = firstname,
                Lastname = lastname,
                IsBlocked = false,
                AskNewPasswordOnNextLogin = passwordNeedsReset,
                Roles = new[]
                {
                    new
                    {
                        Name = "User"
                    }
                }
            });
            return response;
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