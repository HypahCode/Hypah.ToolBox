namespace Hypah.Ai.Provider.Interfaces
{
    public interface IAiConnector
    {
        public string Name { get; }
        public List<string> ConnectionParameters { get; }
        public Task<IAiInterface> Connect(AiModel model, IEnumerable<Parameter> parameters);
        public Task<IEnumerable<AvailableAiModel>> GetAvailableModels();
    }
}
