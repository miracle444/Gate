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
                                                int clientId = BitConverter.ToInt32(data, 0xC);
                                                int mapId = BitConverter.ToInt32(data, 0x10);
                                                var client = Client.AssignGameserverConnection(clientId, connection);

                                                var zone = Zone.CreateInstance(mapId);
                                                zone.AddClient(client);
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