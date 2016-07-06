using Gate.Networking.Servers;

namespace Gate.Controllers
{
        internal interface IController
        {
                void Register(IControllerManager controllerManager);
        }
}