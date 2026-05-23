using Hypah.WebServer.Server;

namespace Hypah.WebServer.Request.Requests
{
    internal class FileRequest : IRequestHandler
    {
        private readonly WebUrl _url;

        public FileRequest(WebUrl url)
        {
            _url = url;
        }

        public bool Handle(IRequestServer server)
        {
            throw new NotImplementedException();
        }
    }
}
