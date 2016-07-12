using System.Collections.Generic;
using Gate.Const;
using Gate.Networking;
using Gate.Networking.Servers;

namespace Gate.Controllers.Game
{
        internal class InstanceLoadController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(139, InstanceLoadRequestItemsHandler);
                        controllerManager.RegisterHandler(130, InstanceLoadRequestMapDataHandler);
                        controllerManager.RegisterHandler(138, InstanceLoadRequestPlayerDataHandler);
                }

                private void InstanceLoadRequestItemsHandler(Connection connection, List<object> message)
                {
                        connection.Send((int) GameServerMessage.CreateInventory, (ushort) 1, (byte) 0);
                        connection.Send((int) GameServerMessage.UpdateActiveWeaponset, (ushort) 1, (byte) 0);

                        //Game.Player.ControlledCharacter.Inventory.LoadItems();

                        connection.Send((int) GameServerMessage.ConnectionStatus, (byte) 0, (ushort) 0x160, 0x85EB21CD);
                }

                private void InstanceLoadRequestMapDataHandler(Connection connection, List<object> message)
                {
                        Client client = Client.FromConnection(connection);

                        connection.Send((int) GameServerMessage.InstanceLoadMapData,
                                        0x28538,
                                        -1873F,
                                        352F,
                                        (ushort) 0,
                                        (byte) 0,
                                        (byte) 0);

                        foreach (int unlockedOutpost in client.ControlledCharacter.UnlockedAreas)
                        {
                                connection.Send((int) GameServerMessage.UnlockArea,
                                                (ushort) unlockedOutpost,
                                                (byte) 0);
                        }
                }

                private void InstanceLoadRequestPlayerDataHandler(Connection connection, List<object> message)
                {
                        Client client = Client.FromConnection(connection);

                        connection.Send((int) GameServerMessage.PlayerData230, 0x61747431);

                        client.Abilities.SendMessage_AttributePoints(client);
                        client.Abilities.SendMessage_Professions(client);
                        client.ControlledCharacter.Skillbar.SendMessage_Skillbar(client);

                        connection.Send((int) GameServerMessage.PlayerData221,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0,
                                        client.ControlledCharacter.Level,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0);

                        connection.Send((int) GameServerMessage.WorldMapDimensions, 0x40, 0x80, 0x1B);
                        connection.Send((int) GameServerMessage.WorldMapData, new uint[]
                                {
                                        0x000B0000, 0x0054FFFF, 0x013A043B, 0x00E8043A,
                                        0x00000000, 0x00000000, 0x17000000
                                });


                        client.Abilities.SendMessage_Attributes(client);
                        client.Abilities.SendMessage_AvailableSkills(client);
                        client.Abilities.SendMessage_AvailableSecondaryProfessions(client);

                        connection.Send((int) GameServerMessage.ControlledCharacter,
                                        client.GetId(client.ControlledCharacter),
                                        3);

                        client.State = GameState.Playing;
                }
        }
}