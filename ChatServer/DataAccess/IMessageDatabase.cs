using System;
using System.Threading.Tasks;
using ChatServer.Model;

namespace ChatServer.DataAccess
{
    public interface IMessageDatabase
    {
        public Task<Message> NewMessage(User sender, Conversation conversation, string text, DateTime time);
        public Task<Message> GetMessage(int id);
    }
}