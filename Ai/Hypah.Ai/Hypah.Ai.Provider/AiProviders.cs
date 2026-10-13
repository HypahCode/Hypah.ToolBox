using Hypah.Ai.Provider.Interfaces;
using System.Reflection;

namespace Hypah.Ai.Provider
{
    public static class AiProviders
    {
        private static readonly List<ConnectedServices> _connectors = new();

        public static void RegisterProvider(string assemblyFilename)
        {
            if (!File.Exists(assemblyFilename)) throw new IOException($"File {assemblyFilename} does not exist");
            var assembly = Assembly.Load(assemblyFilename);

            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IAiConnector).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    var connector = Activator.CreateInstance(type) as IAiConnector;
                    if (connector != null)
                    {
                        _connectors.Add(new ConnectedServices(assembly, connector));
                    }
                }
            }
        }

        public static IEnumerable<IAiConnector> Connectors
        {
            get
            {
                foreach (var connector in _connectors)
                {
                    yield return connector.Connectors;
                }
            }
        }
    }
}
