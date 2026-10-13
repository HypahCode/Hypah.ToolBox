namespace Hypah.Ai.Provider.Interfaces
{
    public interface IAiInterface : IDisposable
    {
        string Name { get; }
        AiModel Model { get; }
        IEnumerable<Parameter> Parameters { get; }
        Task<string> Ask(string prompt);
    }
}
