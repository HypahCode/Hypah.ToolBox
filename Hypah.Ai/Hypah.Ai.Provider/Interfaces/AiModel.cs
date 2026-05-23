namespace Hypah.Ai.Provider.Interfaces
{
    public sealed class AiModel
    {
        public string Name { get; }
        public List<Parameter> Parameters { get; } = new();

        public AiModel(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
