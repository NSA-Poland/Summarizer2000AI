using System.Net.Mime;
using System.Text;

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
            var result = await _httpClient.PostAsync("api/summarize", new StringContent(textToSummarize, Encoding.UTF8, MediaTypeNames.Application.Json));
            return await result.Content.ReadAsStringAsync();
        }
    }
}
