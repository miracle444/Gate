using System;
using System.Collections.Generic;
using System.IO;
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

                internal byte[] GetLoginScreenAppearance()
                {
                        var stream = new MemoryStream();

                        const ushort PACKING_VERSION = 6;
                        stream.Write(BitConverter.GetBytes(PACKING_VERSION), 0, 2);

                        var lastOutpost = (ushort) 0x2AC;
                        stream.Write(BitConverter.GetBytes(lastOutpost), 0, 2);

                        var unknown1 = new byte[]
                                {
                                        0x37, 0x38, 0x31, 0x30 // creation time?
                                };
                        stream.Write(unknown1, 0, unknown1.Length);

                        byte[] appearance = BitConverter.GetBytes(0);
                        stream.Write(appearance, 0, 4);

                        var guildHall = new byte[16];
                        stream.Write(guildHall, 0, guildHall.Length);

                        int campaign = 2;
                        bool isPvp = true;
                        bool showHelm = true;
                        var properties = (ushort) ((byte) campaign | //4 bits
                                                   (14 << 4) | // 5 bits
                                                   (isPvp ? 1 : 0) << 9 | // 1 bit
                                                   (3 << 10) | // 4 bits
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

                private void LoginHandler18(Connection connection, List<object> message)
                {
                        Client client = Client.New(connection);

                        var transactionId = (uint) message[1];

                        if (true)
                        {
                                client.State = GameState.CharacterScreen;

                                connection.Send((int) AuthServerMessage.Character,
                                                transactionId,
                                                new byte[16],
                                                0,
                                                "Gate",
                                                GetLoginScreenAppearance());

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