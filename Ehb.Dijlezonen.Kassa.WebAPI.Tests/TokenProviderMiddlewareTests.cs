using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Tests
{
    public class TokenProviderMiddlewareTests : IntegratedTests
    {
        private readonly ITestOutputHelper output;

        public TokenProviderMiddlewareTests(ITestOutputHelper output) : base(output)
        {
            this.output = output;
        }

        [Fact]
        public async Task CanLoginRegularUserUsingFakeCredentials()
        {
            var rawResponse = await LoginUsingUserCredentials();

            var response = await ParseJsonResponse(rawResponse);

            var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(await GetAccessToken(rawResponse));
            int tokenExpiresInSeconds = response.expires_in;

            accessToken.Should().NotBeNull();
            accessToken.Claims.Should().NotContain(c => c.Type == ClaimTypes.Role && c.Value == "Admin");

            tokenExpiresInSeconds.Should().Be(300);
        }

        [Fact]
        public async Task WithRegularTokenACallCanBeMade()
        {
            var rawResponse = await LoginUsingUserCredentials();

            await SetClientCredentials(rawResponse);

            rawResponse = await Client.GetAsync("/api/test/");
            rawResponse.EnsureSuccessStatusCode();
        }

        private async Task SetClientCredentials(HttpResponseMessage rawResponse)
        {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetAccessToken(rawResponse));
        }

        private async Task<HttpResponseMessage> LoginUsingUserCredentials()
        {
            var response = await Client.PostAsync("/api/token", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "gebruiker"),
                new KeyValuePair<string, string>("password", "gebruiker")
            }));

            response.EnsureSuccessStatusCode();

            return response;
        }

        private async Task<string> GetAccessToken(HttpResponseMessage httpResponse)
        {
            var response = await ParseJsonResponse(httpResponse);

            var accessToken = (string) response.access_token;

            output.WriteLine($"Received token: {accessToken}");

            return accessToken;
        }

        private async Task<dynamic> ParseJsonResponse(HttpResponseMessage httpResponse)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();

            output.WriteLine($"Received web content: {responseString}");

            dynamic response = JsonConvert.DeserializeObject(responseString);
            return response;
        }

        [Fact]
        public async Task WithoutRegularTokenAUserCallCannotBeMade()
        {
            var rawResponse = await Client.GetAsync("/api/test/");
            rawResponse.IsSuccessStatusCode.Should().BeFalse();
            rawResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CanLoginAsAdminUsingFakeCredentials()
        {
            var rawResponse = await LoginUsingAdminCredentials();

            var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(await GetAccessToken(rawResponse));

            accessToken.Should().NotBeNull();
            accessToken.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        }

        private async Task<HttpResponseMessage> LoginUsingAdminCredentials()
        {
            var rawResponse = await Client.PostAsync("/api/token", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "beheerder"),
                new KeyValuePair<string, string>("password", "beheerder")
            }));

            rawResponse.EnsureSuccessStatusCode();

            return rawResponse;
        }

        [Fact]
        public async Task WithAdminTokenAnAdminCallCanBeMade()
        {
            var rawResponse = await LoginUsingAdminCredentials();

            await SetClientCredentials(rawResponse);

            rawResponse = await Client.GetAsync("/api/test/secure-get");
            rawResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WithoutAnAdminTokenAnAdminCallCannotBeMade()
        {
            var rawResponse = await Client.GetAsync("/api/test/secure-get");
            rawResponse.IsSuccessStatusCode.Should().BeFalse();
            rawResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task WithRegularTokenAnAdminCallCannotBeMade()
        {
            var rawResponse = await LoginUsingUserCredentials();

            await SetClientCredentials(rawResponse);

            rawResponse = await Client.GetAsync("/api/test/secure-get");
            rawResponse.IsSuccessStatusCode.Should().BeFalse();
            rawResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task WrongCredentialsReturnsBadRequest()
        {
            var rawResponse = await Client.PostAsync("/api/token", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "TEST"),
                new KeyValuePair<string, string>("password", "WRONG PASSWORD")
            }));

            rawResponse.IsSuccessStatusCode.Should().BeFalse();
            rawResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
