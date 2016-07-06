using System;
using System.Collections.Generic;

namespace Gate.Networking.Servers
{
        internal interface IControllerManager
        {
                void RegisterHandler(int messageId, Action<Connection, List<object>> handler);
        }
}