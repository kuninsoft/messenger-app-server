using System;
using System.Threading.Tasks;
using ChatServer.Model;

namespace ChatServer.DataAccess
{
    public interface IMessageRepository
    {
        public Task<Message> CreateAndGetMessage(User sender, Conversation conversation, string text, DateTime time);
        public Task<int> CreateMessage(User sender, Conversation conversation, string text, DateTime time);
        public Task<Message> GetMessage(int id);
    }
}