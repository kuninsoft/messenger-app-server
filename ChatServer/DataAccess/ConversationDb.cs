using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.DataAccess
{
    public class ConversationDb : IConversationDatabase
    {
        public async Task<bool> CreateConversation(string name, ConversationType type)
        {
            await using var context = new AppContext();

            if (await GetConversation(name) is not null)
            {
                return false;
            }
            
            var conversation = new Conversation(name, type);
            await context.Conversations.AddAsync(conversation);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CheckExistence(string name)
        {
            return await GetConversation(name) is not null;
        }

        public async Task<Conversation> GetConversation(string conversationName)
        {
            await using var context = new AppContext();

            return await context.Conversations
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Name == conversationName);
        }

        public List<Conversation> GetConversationsWithUser(string username)
        {
            using var context = new AppContext();

            return context.Conversations
                .Include(c => c.Users)
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

        public async Task AddUserToConversation(string conversationName, User user)
        {
            await using var context = new AppContext();

            context.Conversations.FirstOrDefault(c => c.Name == conversationName)?.
                Users?.Add(context.Users.FirstOrDefault(u => u.Username == user.Username));

            await context.SaveChangesAsync();
        }

        public async Task AddUsersToConversation(string conversationName, params User[] users)
        {
            foreach (User user in users)
            {
                await AddUserToConversation(conversationName, user);
            }
        }
    }
}