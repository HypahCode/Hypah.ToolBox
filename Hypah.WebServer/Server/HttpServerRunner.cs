using Hypah.WebServer.Request;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Hypah.WebServer.Server
{
    internal class HttpServerRunner : IHttpServer, IDisposable
    {
        private readonly int _port;
        private readonly IWebServer _server;
        private readonly IServerLogger? _logger;
        private readonly RequestRegister _requestRegister;

        public HttpServerRunner(int port, IWebServer server, IServerLogger? logger)
        {
            _port = port;
            _server = server;
            _logger = logger;
            _requestRegister = new RequestRegister(this, _server);
        }

        public RequestRegister RequestRegister => _requestRegister;

        public IEnumerable<Endpoint> Start()
        {
            try
            {
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                BindOnIp(IPAddress.Parse("127.0.0.1"), _port);
                for (int i = 0; i < localIPs.Length; i++)
                {
                    BindOnIp(localIPs[i], _port);
                    _logger?.Log(LogLevel.Debug, nameof(WebServer), $"Web Server Running on: {localIPs[i]}:{_port}");
                }
                return localIPs.Select(x => new Endpoint(x, _port));
            }
            catch (Exception e)
            {
                _logger?.Log(LogLevel.Error, nameof(WebServer), "An Exception Occurred in debug server while Listening :" + e.ToString());
            }
            return Array.Empty<Endpoint>();
        }

        public void Stop()
        {
            _logger?.Log(LogLevel.Debug, nameof(WebServer), "Todo: stop server");
        }

        private void BindOnIp(IPAddress address, int port)
        {
            TcpListener tcpListener = new(address, port);
            tcpListener.Start();

            Thread thread = new(new ThreadStart(delegate { StartListen(tcpListener); }));
            thread.IsBackground = true;
            thread.Start();
        }

        private void StartListen(TcpListener tcpListener)
        {
            while (true)
            {
                Socket socket = tcpListener.AcceptSocket();
                //Engine.Console.LogMessage("HTTP Socket Type " + socket.SocketType);
                if (socket.Connected)
                {
                    //Engine.Console.LogMessage("HTTP Client Connected!! CLient IP " + socket.RemoteEndPoint);
                    Byte[] bReceive = new Byte[2048];
                    int i = socket.Receive(bReceive, bReceive.Length, 0);
                    string buffer = Encoding.ASCII.GetString(bReceive);

                    if (!buffer.StartsWith("GET"))
                    {
                        //Engine.Console.LogMessage("HTTP Only Get Method is supported...");
                        //Engine.Console.LogMessage(buffer);
                        socket.Close();
                        continue;
                    }

                    string bodyMessage = "";
                    string bodyTitle = "";
                    bool finished = false;

                    try
                    {
                        WebUrl url = new(buffer);

                        // First, give the dynamic file handler a chance to handle the request (useful for in-memory images/files)
                        if (_requestRegister.DynamicFileHandler != null)
                        {
                            try
                            {
                                var (Content, Mime, StatusCode) = _requestRegister.DynamicFileHandler(url);
                                if (Content != null)
                                {
                                    // Send the dynamic content and mark finished so other handlers are skipped
                                    SendHeader(Mime ?? "application/octet-stream", Content.Length, StatusCode <= 0 ? 200 : StatusCode, socket);
                                    SendToBrowser(Content, socket);
                                    finished = true;
                                }
                                else if (StatusCode != 0 && StatusCode != 200)
                                {
                                    // No content but explicit status (e.g., 404). Send header with zero content.
                                    SendHeader(Mime ?? "text/html", 0, StatusCode, socket);
                                    finished = true;
                                }
                            }
                            catch (Exception exHandler)
                            {
                                _logger?.Log(LogLevel.Error, nameof(WebServer), "Error in dynamic file handler: " + exHandler.ToString());
                                // fall through to normal handlers or error handling below
                            }
                        }

                        if (!finished)
                        {
                            var page = _requestRegister.GetRequestHandler(url);
                            if (page != null)
                            {
                                var requestServer = new RequestServer(this, socket);
                                finished = page.Handle(requestServer);
                            }
                            else
                            {
                                var requestHandler = _requestRegister.GetDynamicPage(url);
                                if (requestHandler != null)
                                {
                                    var requestServer = new RequestServer(this, socket);
                                    finished = requestHandler.Handle(requestServer);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        bodyTitle = "Error occured";
                        bodyMessage = e.Message;
                    }

                    if (!finished)
                    {
                        //bodyMessage = HtmlBuilder.ConstructSimpleMain(bodyTitle, bodyMessage);
                        string message = bodyMessage;
                        SendHeader("text/html", message.Length, 200, socket);
                        SendToBrowser(message, socket);
                    }
                }
                socket.Close();
            }
        }

        public void Dispose()
        {
            Stop();
        }


        public void SendHeader(string MimeHeader, int contentTotalBytes, int statusCode, Socket socket)
        {
            if (MimeHeader.Length == 0)
            {
                MimeHeader = "text/html";
            }

            string buffer = "";
            buffer = buffer + "HTTP/1.1 " + statusCode.ToString() + "\r\n";
            buffer = buffer + "Server: Hypah Web Server\r\n";
            buffer = buffer + "Content-Type: " + MimeHeader + "\r\n";
            buffer = buffer + "Accept-Ranges: bytes\r\n";
            buffer = buffer + "Content-Length: " + contentTotalBytes + "\r\n\r\n";
            Byte[] data = Encoding.ASCII.GetBytes(buffer);
            SendToBrowser(data, socket);
            //Console.WriteLine("Total Bytes : " + iTotBytes.ToString());
        }

        public void SendToBrowser(String data, Socket socket)
        {
            SendToBrowser(Encoding.ASCII.GetBytes(data), socket);
        }

        public void SendToBrowser(byte[] data, Socket socket)
        {
            int numBytes = 0;
            try
            {
                if (socket.Connected)
                {
                    if ((numBytes = socket.Send(data, data.Length, 0)) == -1)
                    {
                        _logger?.Log(LogLevel.Error, nameof(WebServer), "Socket Error cannot Send Packet");
                    }
                    else
                    {
                        //Engine.Console.LogMessage("No. of bytes send " + numBytes); 
                    }
                }
                else
                {
                    _logger?.Log(LogLevel.Info, nameof(WebServer), "Connection Dropped....");
                }
            }
            catch (Exception e)
            {
                _logger?.Log(LogLevel.Error, nameof(WebServer), "Error Occurred : " + e.ToString());
            }
        }
    }
}
