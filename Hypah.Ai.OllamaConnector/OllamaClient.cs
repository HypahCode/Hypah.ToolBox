using System.Text;

namespace Hypah.Ai.OllamaConnector
{
    /// <summary>
    /// Simple client for calling a local Ollama server.
    /// - Streams response content as it arrives.
    /// - Exposes GenerateAsync which returns the full response text.
    /// </summary>
    public class OllamaClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;
        private readonly string _model;

        public OllamaClient(HttpClient httpClient, string model, string baseUrl = "http://localhost:11434")
        {
            _model = model;
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _endpoint = $"{baseUrl.TrimEnd('/')}/api/generate";
        }

        /// <summary>
        /// Sends the prompt to the Ollama generate endpoint and streams the response until completion.
        /// Returns the accumulated response as a string.
        /// </summary>
        public async Task<string> GenerateAsync(string prompt)
        {
            if (prompt is null)
                throw new ArgumentNullException(nameof(prompt));

            var payload = new
            {
                model = _model,
                prompt = prompt,
                stream = true
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json")
            };

            using var response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var reader = new StreamReader(stream, Encoding.UTF8);

            var sb = new StringBuilder();

            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync().ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var chunk = System.Text.Json.JsonSerializer.Deserialize<OllamaChunk>(line);
                    if (chunk?.response != null)
                        sb.Append(chunk.response);

                    if (chunk?.done == true)
                        break;
                }
                catch
                {
                    // Ignore malformed lines (Ollama sometimes emits partial JSON)
                }
            }

            return sb.ToString();
        }

        private sealed class OllamaChunk
        {
            public string? model { get; set; }
            public string? response { get; set; }
            public bool done { get; set; }
        }

        public void Dispose()
        {
            // Note: _httpClient is not disposed here if passed from caller. Caller manages lifetime.
        }

        public async Task<string> Ask(string prompt)
        {
            return await GenerateAsync(prompt);
        }
    }
}