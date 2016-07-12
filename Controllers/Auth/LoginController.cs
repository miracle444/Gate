using System.Collections.Generic;
using Gate.Const;
using Gate.Networking;
using Gate.Networking.Servers;

namespace Gate.Controllers.Auth
{
        internal class LoginController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(18, LoginHandler18);
                }

                private void LoginHandler18(Connection connection, List<object> message)
                {
                        Client client = Client.New(connection);

                        var transactionId = (uint) message[1];

                        if (true)
                        {
                                client.State = GameState.CharacterScreen;


                                foreach (PlayerCharacter character in client.Characters)
                                {
                                        connection.Send((int) AuthServerMessage.Character,
                                                        transactionId,
                                                        new byte[16],
                                                        0,
                                                        character.Name,
                                                        character.GetLoginScreenAppearance());
                                }

                                connection.Send((int) AuthServerMessage.Gui, transactionId, (ushort) 0);

                                connection.Send((int) AuthServerMessage.PlayerStatus, transactionId, (uint) 1);

                                connection.Send((int) AuthServerMessage.AccountPermissions,
                                                transactionId,
                                                2,
                                                4,
                                                new byte[] {0x38, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
                                                new byte[] {0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x0C, 0x00},
                                                new byte[16],
                                                new byte[16],
                                                8,
                                                new byte[0],
                                                (byte) 23,
                                                0);

                                connection.Send((int) AuthServerMessage.NetError, transactionId, (uint) NetError.NoError);
                        }
                        else
                        {
                                connection.Send((int) AuthServerMessage.NetError, transactionId, (uint) NetError.LoginFailed);
                        }
                }
        }
}