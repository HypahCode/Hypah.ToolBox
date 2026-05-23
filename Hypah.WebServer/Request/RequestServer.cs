using Hypah.WebServer.Server;
using System.Net.Sockets;

namespace Hypah.WebServer.Request
{
    internal class RequestServer : IRequestServer
    {
        private readonly IHttpServer _server;
        private readonly Socket _socket;

        public RequestServer(IHttpServer server, Socket socket)
        {
            _server = server;
            _socket = socket;
        }

        public void SendToBrowser(string mimeHeader, int statusCode, string content)
        {
            _server.SendHeader(mimeHeader, content.Length, statusCode, _socket);
            _server.SendToBrowser(content, _socket);
        }

        public void SendToBrowser(string mimeHeader, int statusCode, byte[] content)
        {
            _server.SendHeader(mimeHeader, content.Length, statusCode, _socket);
            _server.SendToBrowser(content, _socket);
        }
    }
}
