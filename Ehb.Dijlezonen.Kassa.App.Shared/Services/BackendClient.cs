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

        public async Task<HttpResponseMessage> PostForm(string url, IEnumerable<KeyValuePair<string, string>> form, bool requireLogin = false)
        {
            using (var httpClient = GetHttpClient(requireLogin))
            {
                return await httpClient.PostAsync(
                        config.BaseUrl + url,
                        new FormUrlEncodedContent(form));
            }
        }

        public async Task<HttpResponseMessage> PostJson(string url, object body, bool requireLogin = false)
        {
            using (var client = GetHttpClient(requireLogin))
            {
                var postBody = JsonConvert.SerializeObject(body);

                const string mediaType = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
                var content = new StringContent(postBody, Encoding.UTF8, mediaType);

                return await client.PostAsync(
                    config.BaseUrl + url, 
                    content);
            }
        }

        private HttpClient GetHttpClient(bool requireLogin)
        {
            var client = getHttpClient();

            if (requireLogin)
            {
                if (string.IsNullOrEmpty(AccessToken))
                    throw new Exception("You should be logged in first.");

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessToken);
            }

            return client;
        }
    }
}