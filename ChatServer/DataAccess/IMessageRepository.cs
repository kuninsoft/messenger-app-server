using System;
using System.Threading.Tasks;
using ChatServer.Model;

namespace ChatServer.DataAccess
{
    public interface IMessageRepository
    {
        public Task<Message> CreateAndGetMessage(int senderId, int conversationId, string text, DateTime time);
        public Task<int> CreateMessage(int senderId, int conversationId, string text, DateTime time);
        public Task<Message> GetMessage(int id);
    }
}