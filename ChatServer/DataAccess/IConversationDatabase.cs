using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServer.Model;

namespace ChatServer.DataAccess
{
    public interface IConversationDatabase
    {
        public Task<bool> CreateConversation(string name, ConversationType type);
        public Task<bool> CheckExistence(string name);
        public Task<Conversation> GetConversation(string name);
        public List<Conversation> GetConversationsWithUser(string username);
        public Task<Conversation> CreateAndGetConversation(string name, ConversationType type);
        public Task AddUserToConversation(string conversationName, User user);
        public Task AddUsersToConversation(string conversationName, params User[] users);
    }
}