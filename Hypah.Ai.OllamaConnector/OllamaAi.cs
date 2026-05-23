using Hypah.Ai.Provider.Interfaces;

namespace Hypah.Ai.OllamaConnector
{
    public class OllamaAi : IAiInterface
    {
        public string Name => "Ollama";

        public AiModel Model { get; }

        public IEnumerable<Parameter> Parameters { get; }

        private readonly OllamaClient _ollamaClient;
        private readonly HttpClient _httpClient;

        internal OllamaAi(AiModel model)
        {
            Model = model;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(30)
            };
            _ollamaClient = new OllamaClient(_httpClient, model.Name);
        }

        public async Task<string> Ask(string prompt)
        {
            if (_ollamaClient == null) return "";
            return await _ollamaClient.Ask(prompt);
        }

        public void Dispose()
        {
            _ollamaClient.Dispose();
            _httpClient.Dispose();
        }
    }
}
