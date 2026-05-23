using Hypah.Ai.Provider.Interfaces;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Hypah.Ai.OllamaConnector
{
    public class OllamaConnector : IAiConnector
    {
        private readonly string _endpoint;

        public OllamaConnector(string endpoint = "http://localhost:11434")
        {   
            _endpoint = endpoint;
        }

        public string Name => "Ollama";

        public List<string> ConnectionParameters => throw new NotImplementedException();

        public async Task<IAiInterface> Connect(AiModel model, IEnumerable<Parameter> parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AvailableAiModel>> GetAvailableModels()
        {
            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(30)
            };
            var response = await httpClient.GetFromJsonAsync<OllamaTagsResponse>($"{_endpoint}/api/tags");
            if (response?.Models == null)
            {
                throw new Exception($"Could not get AI models from {Name}");
            }

            return response.Models.Select(m => new AvailableAiModel(m.Name));
        }

        private sealed class OllamaTagsResponse
        {
            [JsonPropertyName("models")]
            public List<OllamaModel>? Models { get; set; }
        }

        private sealed class OllamaModel
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
        }
    }
}
