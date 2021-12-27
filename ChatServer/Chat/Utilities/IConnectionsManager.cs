using System.Collections.Generic;
using ChatServer.Model;

namespace ChatServer.Chat.Utilities
{
    public interface IConnectionsManager
    {
        public Dictionary<User, HashSet<string>> UserConnections { get; set; }

        public void MapConnectionToUser(User user, string connectionId);
        public HashSet<string> GetUserConnections(User user);
        public User GetUserByConnection(string connectionId);
        public bool RemoveConnectionFromUser(string connectionId, User user);
    }
}