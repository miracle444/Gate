using System.Collections.Generic;
using Gate.Const;
using Gate.Networking;
using Gate.Networking.Servers;

namespace Gate.Controllers.Auth
{
        internal class MiscController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(9, Packet9Handler);
                        controllerManager.RegisterHandler(10, SelectCharacterHandler);
                        controllerManager.RegisterHandler(32, Packet32Handler);
                        controllerManager.RegisterHandler(41, PlayRequest);
                        controllerManager.RegisterHandler(53, Packet53Handler);
                        controllerManager.RegisterHandler(13, LogoutHandler);
                }

                private void LogoutHandler(Connection connection, List<object> objects)
                {
                        Client client = Client.FromConnection(connection);

                        if (client != null) // can be null at the before login
                        {
                                client.State = GameState.LoginScreen;
                        }
                }

                private void Packet9Handler(Connection connection, List<object> objects)
                {
                        connection.Send((int) AuthServerMessage.NetError, (uint) objects[1], (int) NetError.NoError);
                }

                private void SelectCharacterHandler(Connection connection, List<object> objects)
                {
                        connection.Send((int) AuthServerMessage.NetError, (uint) objects[1], (int) NetError.NoError);
                }

                private void Packet32Handler(Connection connection, List<object> objects)
                {
                        connection.Send((int) AuthServerMessage.NetError, (uint) objects[1], (int) NetError.NoError);
                }

                private void PlayRequest(Connection connection, List<object> objects)
                {
                        Client client = Client.FromConnection(connection);

                        var transactionId = (uint) objects[1];
                        var mapId = (uint) objects[3];

                        if (mapId != 0)
                        {
                                connection.Send((int) AuthServerMessage.Dispatch,
                                                transactionId,
                                                client.ID,
                                                mapId,
                                                new byte[]
                                                        {
                                                                0x02, 0x00, 0x23, 0x98, 0x7F, 0x00, 0x00, 0x01,
                                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                                                        },
                                                0);
                        }
                }

                private void Packet53Handler(Connection connection, List<object> objects)
                {
                        var transactionId = (uint) objects[1];

                        connection.Send(38, transactionId, 0);

                        connection.Send((int) AuthServerMessage.NetError, transactionId, (int) NetError.NoError);
                }
        }
}