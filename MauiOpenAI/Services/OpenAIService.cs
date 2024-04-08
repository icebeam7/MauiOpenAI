using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using System.Net.Http.Headers;

using MauiOpenAI.Helpers;
using MauiOpenAI.Models.Text;
using MauiOpenAI.Models.Images;

namespace MauiOpenAI.Services
{
    public class OpenAIService : IOpenAIService
    {
        HttpClient client;

        public OpenAIService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(OpenAIConstants.AzureServer);
            client.DefaultRequestHeaders.Add("api-key", OpenAIConstants.AzureKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> AskQuestion(string query)
        {
            var completion = new CompletionRequest()
            {
                prompt = query
            };

            var body = JsonSerializer.Serialize(completion);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(OpenAIConstants.CompletionsEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var answer = await response.Content.ReadFromJsonAsync<CompletionResponse>();
                return answer?.choices?.FirstOrDefault()?.text;
            }

            return string.Empty;
        }

        public async Task<string> CreateImage(string query)
        {
            var generation = new GenerationRequest()
            {
                caption = query
            };

            var body = JsonSerializer.Serialize(generation);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var submission = await client.PostAsync(OpenAIConstants.GenerationsEndpoint, content);

            if (submission.IsSuccessStatusCode)
            {
                var headers = submission.Headers;

                var location = headers.GetValues("Operation-Location").FirstOrDefault();
                location = location.Replace(OpenAIConstants.AzureServer, "");

                var retry = int.Parse(headers.GetValues("Retry-after").FirstOrDefault());
                retry *= 1000;

                var status = string.Empty;
                var result = string.Empty;

                while (status != "Succeeded")
                {
                    await Task.Delay(retry);

                    var response = await client.GetAsync(location);

                    if (response.IsSuccessStatusCode)
                    {
                        var answer = await response.Content.ReadFromJsonAsync<GenerationResponse>();
                        status = answer?.status;
                        result = answer?.result?.contentUrl;
                    }
                }

                return result;
            }

            return default;
        }
    }
}
