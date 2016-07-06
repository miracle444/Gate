using System.Collections.Generic;
using System.Linq;
using Gate.Const;

namespace Gate.Networking
{
        internal class Client
        {
                private static int _id;
                private static readonly Dictionary<Connection, Client> _connectionToClientMap = new Dictionary<Connection, Client>();

                private Client(Connection authConnection)
                {
                        ID = _id++;
                        AuthConnection = authConnection;
                        State = GameState.Handshake;
                }

                public int ID { get; private set; }

                public GameState State { get; set; }

                public Connection AuthConnection { get; private set; }
                public Connection GameConnection { get; private set; }

                public static Client New(Connection authConnection)
                {
                        var client = new Client(authConnection);
                        _connectionToClientMap.Add(authConnection, client);
                        return client;
                }

                public static Client FromConnection(Connection connection)
                {
                        Client client;
                        return _connectionToClientMap.TryGetValue(connection, out client) ? client : null;
                }

                public static Client AssignGameserverConnection(uint clientId, Connection connection)
                {
                        Client client = _connectionToClientMap.Values.FirstOrDefault(c => c.ID == clientId);

                        if (client != null)
                        {
                                client.GameConnection = connection;
                                _connectionToClientMap.Add(connection, client);
                        }

                        return client;
                }
        }
}