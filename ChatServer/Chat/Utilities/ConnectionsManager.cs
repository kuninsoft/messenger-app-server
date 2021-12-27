using System;
using System.Collections.Generic;
using System.Linq;
using ChatServer.Model;

namespace ChatServer.Chat.Utilities
{
    public class ConnectionsManager : IConnectionsManager
    {
        public Dictionary<User, HashSet<string>> UserConnections { get; set; } = new();
        public void MapConnectionToUser(User user, string connectionId)
        {
            if (!UserConnections.ContainsKey(user))
            {
                UserConnections.Add(user, new HashSet<string>());
            }
            
            UserConnections[user].Add(connectionId);
        }

        public HashSet<string> GetUserConnections(User user)
        {
            if (!UserConnections.ContainsKey(user))
            {
                return null;
            }
            
            return UserConnections[user];
        }

        public User GetUserByConnection(string connectionId)
        {
            if (!UserConnections.Values.Any(connections => connections.Contains(connectionId)))
            {
                throw new InvalidOperationException();
            }

            return UserConnections.FirstOrDefault(pair => pair.Value.Contains(connectionId)).Key;
        }

        public bool RemoveConnectionFromUser(string connectionId, User user)
        {
            if (!UserConnections.ContainsKey(user))
            {
                throw new InvalidOperationException();
            }

            return UserConnections[user].Remove(connectionId);
        }
    }
}