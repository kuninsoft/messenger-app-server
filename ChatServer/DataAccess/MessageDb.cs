using System;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.DataAccess
{
    public class MessageDb : IMessageDatabase
    {
        public async Task<Message> NewMessage(User sender, Conversation conversation, string text, DateTime time)
        {
            await using var context = new AppContext();

            var message = new Message(text, time)
            {
                Conversation = context.Conversations.FirstOrDefault(c => c.Name == conversation.Name),
                User = context.Users.FirstOrDefault(u => u.Username == sender.Username)
            };

            context.Messages.Add(message);
            await context.SaveChangesAsync();

            return message;
        }

        public async Task<Message> GetMessage(int id)
        {
            await using var context = new AppContext();
            return await context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}