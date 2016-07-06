using System;

namespace Gate.Networking.Servers
{
        internal class GameServer : Server
        {
                public GameServer() : base(Protocols.CtoG, Protocols.GtoC)
                {
                }

                protected override short Port
                {
                        get { return 9112; }
                }

                protected override void Received(Connection connection, byte[] data)
                {
                        switch (BitConverter.ToUInt16(data, 0))
                        {
                                case 1280:
                                        {
                                                uint clientId = BitConverter.ToUInt32(data, 0xC);
                                                Client.AssignGameserverConnection(clientId, connection);
                                        }
                                        break;
                                case 16896:
                                        {
                                                connection.Send(5633, new byte[20]);
                                        }
                                        break;
                                default:
                                        base.Received(connection, data);
                                        break;
                        }
                }
        }
}