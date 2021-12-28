using System;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.DataAccess
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
    
        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Message> CreateAndGetMessage(User sender, Conversation conversation, string text, DateTime time)
        {
            int messageId = await CreateMessage(sender, conversation, text, time);
            Message message = await GetMessage(messageId);
            return message;
        }

        public async Task<int> CreateMessage(User sender, Conversation conversation, string text, DateTime time)
        {
            var message = new Message(text, time)
            {
                Conversation = _context.Conversations.FirstOrDefault(c => c.Name == conversation.Name),
                User = _context.Users.FirstOrDefault(u => u.Username == sender.Username)
            };

            _context.Messages.Add(message);

            await _context.SaveChangesAsync();
            
            return message.Id;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}