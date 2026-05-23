using System.Net;

namespace Hypah.WebServer.Server
{
    public struct Endpoint
    {
        public IPAddress IpAddress { get; }
        public int Port { get; }

        public Endpoint(IPAddress ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }
    }
}
