using System.Collections.Generic;
using Gate.Networking;

namespace Gate
{
        internal class Zone
        {
                private static readonly Dictionary<Client, Zone> clientToZoneMap_ = new Dictionary<Client, Zone>();

                private Zone(int mapId, bool isExplorable)
                {
                        MapId = mapId;
                        IsExplorable = isExplorable;

                        Clients = new List<Client>();
                }

                public int MapId { get; private set; }
                public bool IsExplorable { get; private set; }

                public List<Client> Clients { get; private set; }

                public static Zone CreateInstance(int mapId)
                {
                        return new Zone(mapId, false);
                }

                public static Zone FromClient(Client client)
                {
                        Zone zone;
                        return clientToZoneMap_.TryGetValue(client, out zone) ? zone : null;
                }

                public void AddClient(Client client)
                {
                        Clients.Add(client);
                        clientToZoneMap_.Add(client, this);
                }

                public void RemoveClient(Client client)
                {
                        Clients.Remove(client);
                        clientToZoneMap_.Remove(client);
                }
        }
}