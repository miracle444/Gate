using System;
using System.Net;
using System.Net.Sockets;

namespace Gate.Networking
{
        public sealed class Listener
        {
                private readonly Action<Socket> callback_;
                private readonly Socket socket_;

                public Listener(Action<Socket> callback)
                {
                        callback_ = callback;
                        socket_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }

                public bool Listen(IPEndPoint endPoint)
                {
                        try
                        {
                                socket_.Bind(endPoint);

                                socket_.Listen(100);

                                socket_.BeginAccept(AcceptCallback, null);

                                return true;
                        }
                        catch (Exception)
                        {
                                return false;
                        }
                }

                private void AcceptCallback(IAsyncResult asyncResult)
                {
                        callback_(socket_.EndAccept(asyncResult));
                        socket_.BeginAccept(AcceptCallback, null);
                }
        }
}