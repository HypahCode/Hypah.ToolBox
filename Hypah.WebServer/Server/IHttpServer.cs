using System.Net.Sockets;

namespace Hypah.WebServer.Server
{
    internal interface IHttpServer
    {
        void SendHeader(string MimeHeader, int contentTotalBytes, int statusCode, Socket socket);
        void SendToBrowser(string data, Socket socket);
        void SendToBrowser(byte[] data, Socket socket);
    }
}
