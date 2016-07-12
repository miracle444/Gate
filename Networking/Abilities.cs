using Gate.Const;

namespace Gate.Networking
{
        internal class Abilities
        {
                private readonly Client client_;

                public Abilities(Client client)
                {
                        client_ = client;

                        Primary = 1;
                        Secondary = 2;
                }

                public int FreeAttributePoints { get; private set; }

                public int Primary { get; private set; }
                public int Secondary { get; private set; }

                public void SendMessage_AttributePoints(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.SetAttributePoints,
                                                   client.GetId(client_.ControlledCharacter, true),
                                                   (byte) FreeAttributePoints,
                                                   (byte) 0);
                }

                public void SendMessage_Professions(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.UpdatePrivateProfessions,
                                                   client.GetId(client_.ControlledCharacter),
                                                   (byte) Primary,
                                                   (byte) Secondary,
                                                   (byte) 1);
                }

                public void SendMessage_Attributes(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.UpdateAttributes,
                                                   client.GetId(client_.ControlledCharacter),
                                                   new int[0]);
                }

                public void SendMessage_AvailableSkills(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.UpdateAvailableSkills, new int[0]);
                }

                public void SendMessage_AvailableSecondaryProfessions(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.UpdateAvailableProfessions,
                                                   client.GetId(client_.ControlledCharacter),
                                                   1);
                }
        }
}