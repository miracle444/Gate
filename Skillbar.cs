using Gate.Const;
using Gate.Networking;

namespace Gate
{
        internal class Skillbar
        {
                private readonly PlayerCharacter playerCharacter_;

                public Skillbar(PlayerCharacter playerCharacter)
                {
                        playerCharacter_ = playerCharacter;

                        Skills = new int[8];
                        SkillCopies = new int[8];
                }

                public int[] Skills { get; private set; }
                public int[] SkillCopies { get; private set; }

                public void SendMessage_Skillbar(Client client)
                {
                        client.GameConnection.Send((int) GameServerMessage.UpdateSkillBar,
                                                   client.GetId(playerCharacter_),
                                                   Skills,
                                                   SkillCopies,
                                                   (byte) 1);
                }
        }
}