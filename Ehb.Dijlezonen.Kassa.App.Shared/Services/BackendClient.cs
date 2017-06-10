using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class BackendClient
    {
        private readonly IBackendConfiguration config;
        private readonly Func<HttpClient> getHttpClient;

        public string AccessToken { get; set; }

        public BackendClient(IBackendConfiguration config, Func<HttpClient> getHttpClient)
        {
            this.config = config;
            this.getHttpClient = getHttpClient;
        }

        public async Task<BackendClientCallResult> PostForm(string url, IEnumerable<KeyValuePair<string, string>> form)
        {
            using (var httpClient = GetHttpClient())
            {
                var response = await httpClient.PostAsync(
                        config.BaseUrl + url,
                        new FormUrlEncodedContent(form));

                response.EnsureSuccessStatusCode();

                return new BackendClientCallResult(response);
            }
        }

        public async Task<BackendClientCallResult> PostJson(string url, object body)
        {
            using (var client = GetHttpClient())
            {
                var postBody = JsonConvert.SerializeObject(body);

                const string mediaType = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
                var content = new StringContent(postBody, Encoding.UTF8, mediaType);

                var response = await client.PostAsync(
                    config.BaseUrl + url, 
                    content);

                response.EnsureSuccessStatusCode();

                return new BackendClientCallResult(response);
            }
        }

        private HttpClient GetHttpClient()
        {
            var client = getHttpClient();

            if (!string.IsNullOrEmpty(AccessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessToken);
            }

            return client;
        }
    }

    public class BackendClientCallResult
    {
        private readonly HttpResponseMessage response;

        public BackendClientCallResult(HttpResponseMessage response)
        {
            this.response = response;
        }

        public Task<string> GetContent => response.Content.ReadAsStringAsync();
    }
}