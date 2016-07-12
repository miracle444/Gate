using System;
using Gate.Const;
using Gate.Controllers.Game;

namespace Gate.Networking.Servers
{
        internal class GameServer : Server
        {
                public GameServer() : base(Protocols.CtoG, Protocols.GtoC)
                {
                        RegisterController(new InstanceLoadController());
                        RegisterController(new MiscController());
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
                                                Client client = Client.AssignGameserverConnection(clientId, connection);
                                                Zone zone = Zone.CreateInstance(mapId);
                                                zone.AddClient(client);
                                        }
                                        break;
                                case 16896:
                                        {
                                                Client client = Client.FromConnection(connection);
                                                Zone zone = Zone.FromClient(client);

                                                connection.Send(5633, new byte[20]);

                                                if (client.State == GameState.CharacterCreation)
                                                {
                                                        connection.Send((int) GameServerMessage.BeginCharacterCreation);
                                                }
                                                else
                                                {
                                                        connection.Send((int) GameServerMessage.InstanceLoadHead,
                                                                        (byte) 0x3F,
                                                                        (byte) 0x3F,
                                                                        (byte) 0,
                                                                        (byte) 0);

                                                        connection.Send((int) GameServerMessage.InstanceLoadDistrictInfo,
                                                                        client.GetId(client.ControlledCharacter, true),
                                                                        (ushort) zone.MapId,
                                                                        (byte) (zone.IsExplorable ? 1 : 0),
                                                                        (ushort) 1,
                                                                        (ushort) 0,
                                                                        (byte) 0,
                                                                        (byte) 0);
                                                }
                                        }
                                        break;
                                default:
                                        base.Received(connection, data);
                                        break;
                        }
                }
        }
}