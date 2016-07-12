using System.Collections.Generic;
using Gate.Const;
using Gate.Networking;
using Gate.Networking.Servers;

namespace Gate.Controllers.Game
{
        internal class MiscController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(2, ExitToCharacterScreenHandler);
                        controllerManager.RegisterHandler(3, ExitToLoginScreenHandler);
                }

                private void ExitToCharacterScreenHandler(Connection connection, List<object> message)
                {
                        Client client = Client.FromConnection(connection);

                        client.State = GameState.CharacterScreen;
                        client.DropGameConnection();
                }

                private void ExitToLoginScreenHandler(Connection connection, List<object> message)
                {
                        Client client = Client.FromConnection(connection);

                        client.State = GameState.LoginScreen;
                        client.DropGameConnection();
                }
        }
}