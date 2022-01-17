using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServer.Model;

namespace ChatServer.DataAccess
{
    public interface IConversationRepository
    {
        public Task<bool> CreateConversation(string name, ConversationType type);
        public Task<bool> CheckExistence(string name);
        public Task<Conversation> GetConversation(int id);
        public Task<Conversation> GetConversation(string name);
        public Task<Conversation> CreateAndGetConversation(string name, ConversationType type);
        public Task<bool> AddUserToConversation(int conversationId, int userId);
        public Task<bool> AddUsersToConversation(int conversationId, params int[] userIds);
    }
}