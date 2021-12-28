using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServer.Model;

namespace ChatServer.DataAccess
{
    public interface IConversationRepository
    {
        public Task<bool> CreateConversation(string name, ConversationType type);
        public Task<bool> CheckExistence(string name);
        public Task<Conversation> GetConversation(string name);
        public List<Conversation> GetConversationsWithUser(string username);
        public Task<Conversation> CreateAndGetConversation(string name, ConversationType type);
        public Task<bool> AddUserToConversation(string conversationName, User user);
        public Task<bool> AddUsersToConversation(string conversationName, params User[] users);
    }
}