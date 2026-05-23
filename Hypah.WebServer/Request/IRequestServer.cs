
namespace Hypah.WebServer.Request
{
    public interface IRequestServer
    {
        public void SendToBrowser(string mimeHeader, int statusCode, string content);
        public void SendToBrowser(string mimeHeader, int statusCode, byte[] content);
    }
}
