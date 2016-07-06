using System;
using Gate.Controllers.Auth;

namespace Gate.Networking.Servers
{
        internal class AuthServer : Server
        {
                public AuthServer() : base(Protocols.CtoA, Protocols.AtoC)
                {
                        RegisterController(new ComputerInfoController());
                        RegisterController(new LoginController());
                        RegisterController(new MiscController());
                }

                protected override short Port
                {
                        get { return 6112; }
                }

                protected override void Received(Connection connection, byte[] data)
                {
                        switch (BitConverter.ToUInt16(data, 0))
                        {
                                case 1024:
                                        break;
                                case 16896:
                                        connection.Send(5633, new byte[20]);
                                        break;
                                default:
                                        base.Received(connection, data);
                                        break;
                        }
                }
        }
}