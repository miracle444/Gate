using System;
using Gate.Networking.Servers;

namespace Gate
{
        public sealed class Gate
        {
                private static volatile Gate _instance;
                private static readonly object _lockObject = new Object();

                private readonly AuthServer authServer_;
                private readonly GameServer gameServer_;

                private Gate()
                {
                        authServer_ = new AuthServer();
                        gameServer_ = new GameServer();
                }

                public static Gate Instance
                {
                        get
                        {
                                if (_instance == null)
                                {
                                        lock (_lockObject)
                                        {
                                                if (_instance == null)
                                                {
                                                        _instance = new Gate();
                                                }
                                        }
                                }

                                return _instance;
                        }
                }

                public void StartServers()
                {
                        authServer_.Start();
                        gameServer_.Start();
                }
        }
}