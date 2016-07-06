using System.Collections.Generic;
using Gate.Const;
using Gate.Networking;
using Gate.Networking.Servers;

namespace Gate.Controllers.Auth
{
        internal class ComputerInfoController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(2, ComputerInfoHandler);
                }

                private void ComputerInfoHandler(Connection connection, List<object> message)
                {
                        connection.Send((int) AuthServerMessage.ComputerInfoReply, 0, 0, 1, 1);
                }
        }
}