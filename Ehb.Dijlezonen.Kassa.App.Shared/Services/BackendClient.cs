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

        public async Task<HttpResponseMessage> PostForm(string url, IEnumerable<KeyValuePair<string, string>> form)
        {
            using (var httpClient = GetHttpClient())
            {
                return await httpClient.PostAsync(
                        config.BaseUrl + url,
                        new FormUrlEncodedContent(form));
            }
        }

        public async Task<HttpResponseMessage> PostJson(string url, object body)
        {
            using (var client = GetHttpClient())
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
}