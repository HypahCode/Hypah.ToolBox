using Hypah.Ai.Provider.Interfaces;
using System.Reflection;

namespace Hypah.Ai.Provider
{
    internal sealed class ConnectedServices
    {
        internal Assembly Assembly { get; }

        internal IAiConnector Connectors { get; }

        public ConnectedServices(Assembly assembly, IAiConnector connectors)
        {
            Assembly = assembly;
            Connectors = connectors;
        }
    }
}
