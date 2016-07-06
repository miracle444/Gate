using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Gate.Controllers;

namespace Gate.Networking.Servers
{
        internal abstract class Server : IControllerManager
        {
                private readonly List<Connection> connections_ = new List<Connection>();
                private readonly Dictionary<int, Action<Connection, List<object>>> controllers_ = new Dictionary<int, Action<Connection, List<object>>>();
                private readonly Dictionary<int, int[]> ctoSProtocol_;
                private readonly Listener listener_;
                private readonly Dictionary<int, int[]> stoCProtocol_;

                protected Server(Dictionary<int, int[]> ctoSProtocol, Dictionary<int, int[]> stoCProtocol)
                {
                        ctoSProtocol_ = ctoSProtocol;
                        stoCProtocol_ = stoCProtocol;
                        listener_ = new Listener(SocketAccepted);
                }

                protected abstract short Port { get; }

                public void RegisterHandler(int messageId, Action<Connection, List<object>> handler)
                {
                        controllers_.Add(messageId, handler);
                }

                private void SocketAccepted(Socket socket)
                {
                        var connection = new Connection(socket, Received, stoCProtocol_);
                        connection.BeginReceiving();
                        connections_.Add(connection);
                }

                protected void RegisterController(IController controller)
                {
                        controller.Register(this);
                }

                public void Start()
                {
                        listener_.Listen(new IPEndPoint(IPAddress.Any, Port));
                }

                protected virtual void Received(Connection connection, byte[] data)
                {
                        List<object> message;
                        while (Deserialize(data, out message, out data))
                        {
                                var messageId = (int) message[0];

                                Console.WriteLine(GetType().Name + ": " + messageId);

                                if (controllers_.ContainsKey(messageId))
                                {
                                        try
                                        {
                                                controllers_[messageId](connection, message);
                                        }
                                        catch (Exception e)
                                        {
                                                throw e;
                                        }
                                }

                                if (data.Length == 0) break;
                        }
                }

                private bool Deserialize(byte[] data, out List<object> message, out byte[] remainingData)
                {
                        using (var stream = new MemoryStream(data))
                        using (var reader = new BinaryReader(stream))
                        {
                                var messageId = (int) reader.ReadUInt16();

                                message = new List<object> {messageId};

                                foreach (int parameter in ctoSProtocol_[messageId])
                                {
                                        switch (parameter & 0xF)
                                        {
                                                case 0:
                                                        message.Add(reader.ReadUInt32());
                                                        break;
                                                case 1:
                                                        message.Add(reader.ReadSingle());
                                                        break;
                                                case 2:
                                                        message.Add(new[] {reader.ReadSingle(), reader.ReadSingle()});
                                                        break;
                                                case 3:
                                                        message.Add(new[] {reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()});
                                                        break;
                                                case 4:
                                                        switch (parameter >> 8)
                                                        {
                                                                case sizeof (Byte):
                                                                        message.Add(reader.ReadByte());
                                                                        break;
                                                                case sizeof (UInt16):
                                                                        message.Add(reader.ReadUInt16());
                                                                        break;
                                                                case sizeof (UInt32):
                                                                        message.Add(reader.ReadUInt32());
                                                                        break;
                                                                default:
                                                                        throw new ArgumentException();
                                                        }
                                                        break;
                                                case 5:
                                                case 9:
                                                        message.Add(reader.ReadBytes(parameter >> 8));
                                                        break;
                                                case 7:
                                                        var s = new char[reader.ReadUInt16()];
                                                        for (int j = 0; j < s.Length; j++)
                                                                s[j] = (char) reader.ReadUInt16();
                                                        message.Add(new string(s));
                                                        break;
                                                case 11:
                                                        ushort length = reader.ReadUInt16();

                                                        switch ((parameter >> 4) & 0xF)
                                                        {
                                                                case 0:
                                                                        message.Add(reader.ReadBytes(length));
                                                                        break;
                                                                case 1:
                                                                        var sa = new ushort[length];
                                                                        for (int j = 0; j < length; j++)
                                                                                sa[j] = reader.ReadUInt16();
                                                                        message.Add(sa);
                                                                        break;
                                                                case 2:
                                                                        var ia = new uint[length];
                                                                        for (int j = 0; j < length; j++)
                                                                                ia[j] = reader.ReadUInt32();
                                                                        message.Add(ia);
                                                                        break;
                                                                default:
                                                                        throw new ArgumentException();
                                                        }
                                                        break;
                                                case 6:
                                                case 10:
                                                        break;
                                        }
                                }

                                remainingData = reader.ReadBytes((int) (stream.Length - stream.Position));
                                return true;
                        }
                }
        }
}