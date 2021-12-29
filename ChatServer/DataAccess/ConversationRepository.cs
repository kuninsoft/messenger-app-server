using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.DataAccess
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly AppDbContext _context;

        public ConversationRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> CreateConversation(string name, ConversationType type)
        {
            if (await GetConversation(name) is not null)
            {
                return false;
            }
            
            var conversation = new Conversation(name, type);
            await _context.Conversations.AddAsync(conversation);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CheckExistence(string name)
        {
            return await GetConversation(name) is not null;
        }

        public async Task<Conversation> GetConversation(string conversationName)
        {
            return await _context.Conversations.FirstOrDefaultAsync(c => c.Name == conversationName);
        }

        public List<Conversation> GetConversationsWithUser(string username)
        {
            return _context.Conversations
                .Where(c => c.Users.FirstOrDefault(u => u.Username == username) != null)
                .ToList();
        }

        public async Task<Conversation> CreateAndGetConversation(string name, ConversationType type)
        {
            if (!await CreateConversation(name, type))
            {
                throw new InvalidOperationException();
            }
            
            return await GetConversation(name);
        }

        public async Task<bool> AddUserToConversation(string conversationName, User user)
        {
            _context.Conversations.FirstOrDefault(c => c.Name == conversationName)?.
                Users?.Add(_context.Users.FirstOrDefault(u => u.Username == user.Username));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddUsersToConversation(string conversationName, params User[] users)
        {
            foreach (User user in users)
            {
                bool successful = await AddUserToConversation(conversationName, user);
                if (!successful) return false;
            }

            return true;
        }
    }
}