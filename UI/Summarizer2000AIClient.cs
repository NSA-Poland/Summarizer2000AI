using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace UI
{
    public class Summarizer2000AIClient
    {
        private readonly HttpClient _httpClient;

        public Summarizer2000AIClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetSummarize(string textToSummarize)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { Text = textToSummarize }), Encoding.UTF8, MediaTypeNames.Application.Json);
            var result = await _httpClient.PostAsync("api/summarize", content);
            return await result.Content.ReadAsStringAsync();
        }
    }
}
