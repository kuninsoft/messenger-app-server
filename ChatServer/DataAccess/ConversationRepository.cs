using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.DataAccess.EFCore;
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

        public async Task<Conversation> GetConversation(int id)
        {
            return await _context.Conversations.FindAsync(id);
        }

        public async Task<Conversation> GetConversation(string conversationName)
        {
            return await _context.Conversations.AsQueryable().FirstOrDefaultAsync(c => c.Name == conversationName);
        }

        public async Task<Conversation> CreateAndGetConversation(string name, ConversationType type)
        {
            if (!await CreateConversation(name, type))
            {
                throw new InvalidOperationException();
            }
            
            return await GetConversation(name);
        }

        public async Task<bool> AddUserToConversation(int conversationId, int userId)
        {
            var conversation = await GetConversation(conversationId);
            conversation.Users?.Add(await _context.Users.FindAsync(userId));

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

        public async Task<bool> AddUsersToConversation(int conversationId, params int[] userIds)
        {
            foreach (int userId in userIds)
            {
                bool successful = await AddUserToConversation(conversationId, userId);
                if (!successful) return false;
            }

            return true;
        }
    }
}