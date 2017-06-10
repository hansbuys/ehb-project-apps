using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Tests
{
    public class TokenProviderMiddlewareTests : IntegratedTests
    {
        public TokenProviderMiddlewareTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CanLoginRegularUserUsingFakeCredentials()
        {
            var rawResponse = await DoLoginUsingUserCredentials();
            rawResponse.EnsureSuccessStatusCode();

            var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(await GetAccessToken(rawResponse));

            accessToken.Should().NotBeNull();
            accessToken.Claims.Should().NotContain(c => c.Type == ClaimTypes.Role && c.Value.Contains("Admin"));
            accessToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task WithRegularTokenACallCanBeMade()
        {
            await LoginUsingUserCredentials();

            var response = await Get("/api/test/");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WithoutRegularTokenAUserCallCannotBeMade()
        {
            var response = await Get("/api/test/");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CanLoginAsAdminUsingFakeCredentials()
        {
            var response = await DoLoginUsingAdminCredentials();
            response.EnsureSuccessStatusCode();

            var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(await GetAccessToken(response));

            accessToken.Should().NotBeNull();
            accessToken.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        }

        [Fact]
        public async Task WithAdminTokenAnAdminCallCanBeMade()
        {
            await LoginUsingAdminCredentials();

            var response = await Get("/api/test/secure-get");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WithoutAnAdminTokenAnAdminCallCannotBeMade()
        {
            var response = await Get("/api/test/secure-get");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task WithRegularTokenAnAdminCallCannotBeMade()
        {
            await LoginUsingUserCredentials();

            var response = await Get("/api/test/secure-get");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData("gebruiker")]
        [InlineData("beheerder")]
        [InlineData("test")]
        public async Task WrongCredentialsCannotLogin(string username)
        {
            var response =  await DoLoginUsingCredentials(username, "WRONG PASSWORD");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
