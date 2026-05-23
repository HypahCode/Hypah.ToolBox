namespace Hypah.Ai.Provider.Interfaces
{
    public sealed class AvailableAiModel
    {
        public string Name { get; }
        public List<ParameterDefinition> Parameters { get; } = new();
        public AvailableAiModel(string name)
        {
            Name = name;
        }

        public AiModel CreateAiModel()
        {
            var aiModel = new AiModel(Name);
            foreach (var paramDef in Parameters)
            {
                aiModel.Parameters.Add(paramDef.CreateDefaultParameter());
            }
            return aiModel;
        }
    }
}
