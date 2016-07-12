using System.Collections.Generic;
using System.Linq;
using Gate.Const;

namespace Gate.Networking
{
        internal class Client
        {
                private static int _id;
                private static readonly Dictionary<Connection, Client> connectionToClientMap_ = new Dictionary<Connection, Client>();

                internal readonly Abilities Abilities;
                private readonly HashSet<object> objectCreatedSet_ = new HashSet<object>();
                private readonly Dictionary<object, int> objectToIdMap_ = new Dictionary<object, int>();
                private int nextId_ = 1;

                private Client(Connection authConnection)
                {
                        Abilities = new Abilities(this);

                        ID = _id++;
                        AuthConnection = authConnection;
                        State = GameState.Handshake;
                }

                internal int ID { get; private set; }

                internal GameState State { get; set; }

                internal Connection AuthConnection { get; private set; }
                internal Connection GameConnection { get; private set; }

                internal PlayerCharacter ControlledCharacter { get; private set; }
                internal IEnumerable<PlayerCharacter> Characters { get; private set; }

                internal static Client New(Connection authConnection)
                {
                        var characters = new List<PlayerCharacter>();
                        for (int i = 0; i < 20; i++) characters.Add(new PlayerCharacter("Gate" + i));
                        var client = new Client(authConnection)
                                {
                                        Characters = characters,
                                        ControlledCharacter = characters.First()
                                };
                        connectionToClientMap_.Add(authConnection, client);
                        return client;
                }

                internal static Client FromConnection(Connection connection)
                {
                        Client client;
                        return connectionToClientMap_.TryGetValue(connection, out client) ? client : null;
                }

                internal static Client AssignGameserverConnection(int clientId, Connection connection)
                {
                        Client client = connectionToClientMap_.Values.FirstOrDefault(c => c.ID == clientId);

                        if (client != null)
                        {
                                client.GameConnection = connection;
                                connectionToClientMap_.Add(connection, client);
                        }

                        return client;
                }

                internal int GetId(Identifyable obj, bool prefetch = false)
                {
                        if (objectCreatedSet_.Contains(obj)) return objectToIdMap_[obj];

                        int id;
                        if (!objectToIdMap_.TryGetValue(obj, out id))
                        {
                                id = nextId_++;
                                objectToIdMap_.Add(obj, id);
                        }

                        if (!prefetch)
                        {
                                objectCreatedSet_.Add(obj); // mark object as created before creating it to prevent recursion
                                obj.Create(this);
                        }

                        return id;
                }

                internal void SelectCharacter(string name)
                {
                        ControlledCharacter = Characters.First(c => c.Name.Equals(name));
                }

                internal void DropGameConnection()
                {
                        Zone.FromClient(this).RemoveClient(this);

                        GameConnection.Disconnect();
                        connectionToClientMap_.Remove(GameConnection);
                        GameConnection = null;
                }
        }
}