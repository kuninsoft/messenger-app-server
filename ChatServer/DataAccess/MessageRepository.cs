using System;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.DataAccess.EFCore;
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
        
        public async Task<Message> CreateAndGetMessage(int senderId, int conversationId, string text, DateTime time)
        {
            int messageId = await CreateMessage(senderId, conversationId, text, time);
            Message message = await GetMessage(messageId);
            return message;
        }

        public async Task<int> CreateMessage(int senderId, int conversationId, string text, DateTime time)
        {
            var message = new Message(text, time)
            {
                Conversation = await _context.Conversations.FindAsync(conversationId),
                User = await _context.Users.FindAsync(senderId)
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