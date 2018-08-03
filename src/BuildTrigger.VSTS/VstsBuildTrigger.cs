using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BuildTrigger;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace BuildTrigger.VSTS
{
    public class VstsBuildTrigger : IBuildTrigger
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _baseUrl;

        public VstsBuildTrigger(IOptions<VstsBuildOptions> options)
        {
            _baseUrl = $"https://{options.Value.Instance}.visualstudio.com/DefaultCollection/{options.Value.Project}/_apis/build";
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", options.Value.TokenBase64);
        }

        public async Task<List<Build>> GetBuildsAsync()
        {
            var url = "/definitions?api-version=2.0";
            var response = new List<Build>();
            var result = await _client.GetAsync($"{_baseUrl}{url}");
            if (result.IsSuccessStatusCode)
            {
                var raw = await result.Content.ReadAsStringAsync();
                var buildDefinitions = JsonConvert.DeserializeObject<VstsResponse>(raw);
                foreach(var buildDef in buildDefinitions.value)
                {
                    response.Add(new Build
                    {
                        Id = buildDef.id,
                        Name = buildDef.name
                    });
                }
            }

            return response;
        }

        public Task<HttpResponseMessage> TriggerAsync(int buildNumber)
        {
            var url = "/builds?api-version=2.0";
            var buildArg = new BuildTriggerArg
            {
                Definition = new BuildDefinition{
                    Id = buildNumber
                }
            };

            var body = JsonConvert.SerializeObject(buildArg);
            return _client.PostAsync($"{_baseUrl}{url}", new StringContent(body, Encoding.UTF8, "application/json"));
        }

        private class BuildDefinition
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }
        }

        private class BuildTriggerArg
        {
            [JsonProperty(PropertyName = "definition")]
            public BuildDefinition Definition { get; set; }
        }
    }
}
