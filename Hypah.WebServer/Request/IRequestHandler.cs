
using Hypah.WebServer.Server;

namespace Hypah.WebServer.Request
{
    public interface IRequestHandler
    {
        bool Handle(IRequestServer server);
    }
}
