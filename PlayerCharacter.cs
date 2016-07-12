using System;
using System.IO;
using System.Linq;
using System.Text;
using Gate.Const;
using Gate.Networking;

namespace Gate
{
        internal class PlayerCharacter : Identifyable
        {
                internal readonly PlayerAppearance Appearance;
                internal readonly Skillbar Skillbar;

                internal PlayerCharacter(string name)
                {
                        Skillbar = new Skillbar(this);
                        Appearance = PlayerAppearance.Random();

                        Name = name;

                        Speed = 288F;

                        Level = 21;
                        Morale = 100;

                        X = -1873F;
                        Y = 352F;

                        Primary = 1;
                        Secondary = 2;

                        UnlockedAreas = Enumerable.Range(1, 877).ToArray();
                }

                internal string Name { get; private set; }

                internal float X { get; private set; }
                internal float Y { get; private set; }
                internal float Speed { get; private set; }

                internal int Primary { get; private set; }
                internal int Secondary { get; private set; }

                internal int Level { get; private set; }
                internal int Status { get; private set; }
                internal int Morale { get; private set; }

                internal int[] UnlockedAreas { get; private set; }

                internal void SendMessage_Appearance(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.UpdateAgentAppearance,
                                                   client.GetId(this),
                                                   client.GetId(this),
                                                   Appearance.GetPackedValue(),
                                                   (byte) 0,
                                                   0x800,
                                                   0,
                                                   Name);
                }

                internal void SendMessage_Professions(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.AgentProfessions,
                                                   client.GetId(this),
                                                   (byte) Primary,
                                                   (byte) Secondary);
                }

                internal void SendMessage_Level(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.AgentPropertyInt,
                                                   (int) AgentProperty.Level,
                                                   client.GetId(this),
                                                   Level);
                }

                internal void SendMessage_Status(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.AgentStatus,
                                                   client.GetId(this),
                                                   Status);
                }

                internal void SendMessage_MaximumHealth(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.AgentPropertyInt,
                                                   (int) AgentProperty.MaximumHealth,
                                                   client.GetId(this),
                                                   1);
                }

                internal void SendMessage_CurrentHealth(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.AgentPropertyFloat,
                                                   (int) AgentProperty.CurrentHealth,
                                                   client.GetId(this),
                                                   1);
                }

                internal void SendMessage_MaximumEnergy(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.AgentPropertyInt,
                                                   (int) AgentProperty.MaximumEnergy,
                                                   client.GetId(this),
                                                   1);
                }

                internal void SendMessage_CurrentEnergy(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.AgentPropertyFloat,
                                                   (int) AgentProperty.CurrentEnergy,
                                                   client.GetId(this),
                                                   1);
                }

                internal void SendMessage_Spawn(Client client, int agentType, bool allied = true)
                {
                        uint a = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("MONS"), 0);

                        client.GameConnection.Send((int) GameServerMessage.SpawnAgent,
                                                   client.GetId(this),
                                                   agentType | client.GetId(this),
                                                   (byte) (agentType > 0 ? 1 : 4),
                                                   (byte) 5,
                                                   X,
                                                   Y,
                                                   (ushort) 0,
                                                   (float) 0,
                                                   (float) 0,
                                                   (byte) 1,
                                                   Speed,
                                                   float.PositiveInfinity,
                                                   0,
                                                   (allied ? 0x61747431 : a),
                                                   0,
                                                   0,
                                                   0,
                                                   0,
                                                   0,
                                                   (float) 0,
                                                   (float) 0,
                                                   float.PositiveInfinity,
                                                   float.PositiveInfinity,
                                                   (ushort) 0,
                                                   0,
                                                   float.PositiveInfinity,
                                                   float.PositiveInfinity,
                                                   (ushort) 0);
                }

                internal void SendMessage_Morale(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.AgentMorale,
                                                   client.GetId(this),
                                                   Morale);
                }

                internal override void Create(Client client)
                {
                        SendMessage_Appearance(client);
                        SendMessage_Professions(client);
                        SendMessage_Level(client);
                        SendMessage_Status(client);
                        SendMessage_MaximumHealth(client);
                        SendMessage_CurrentHealth(client);
                        SendMessage_MaximumEnergy(client);
                        SendMessage_CurrentEnergy(client);
                        SendMessage_Spawn(client, 0x30000000);
                        /*connection.Send((int) GameServerMessage.UpdateFullEquipment,
                                        zone.GetId(client.ControlledCharacter),
                                        0, 0, 0, 0, 0, 0, 0, 0, 0);*/
                        SendMessage_Morale(client);
                }

                internal byte[] GetLoginScreenAppearance()
                {
                        var stream = new MemoryStream();

                        const ushort PACKING_VERSION = 6;
                        stream.Write(BitConverter.GetBytes(PACKING_VERSION), 0, 2);

                        var lastOutpost = (ushort) 189;
                        stream.Write(BitConverter.GetBytes(lastOutpost), 0, 2);

                        var unknown1 = new byte[]
                                {
                                        0x37, 0x38, 0x31, 0x30 // creation time?
                                };
                        stream.Write(unknown1, 0, unknown1.Length);

                        byte[] appearance = BitConverter.GetBytes(Appearance.GetPackedValue());
                        stream.Write(appearance, 0, 4);

                        var guildHall = new byte[16];
                        stream.Write(guildHall, 0, guildHall.Length);

                        int campaign = 2;
                        bool isPvp = true;
                        bool showHelm = true;
                        var properties = (ushort) ((byte) campaign | //4 bits
                                                   (Level << 4) | // 5 bits
                                                   (isPvp ? 1 : 0) << 9 | // 1 bit
                                                   ((byte) Secondary << 10) | // 4 bits
                                                   (showHelm ? 1 : 0) << 14); // 1 bit
                        // 1 bit left
                        stream.Write(BitConverter.GetBytes(properties), 0, 2);

                        var unknown2 = new byte[]
                                {
                                        0xDD, 0xDD, // ?
                                        0x05, // # equipment pieces
                                        0xDD, 0xDD, 0xDD, 0xDD // ?
                                };
                        stream.Write(unknown2, 0, unknown2.Length);

                        stream.Write(new byte[4], 0, 4);
                        stream.WriteByte(1);

                        stream.Write(new byte[4], 0, 4);
                        stream.WriteByte(1);

                        stream.Write(new byte[4], 0, 4);
                        stream.WriteByte(1);

                        stream.Write(new byte[4], 0, 4);
                        stream.WriteByte(1);

                        stream.Write(new byte[4], 0, 4);
                        stream.WriteByte(1);

                        return stream.ToArray();
                }
        }
}