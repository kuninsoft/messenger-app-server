using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.DataAccess.EFCore;
using ChatServer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatServer.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        
        public async Task<User> GetUser(string username)
        {
            return await _context.Users.AsQueryable().FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> CreateUser(string username, string password)
        {
            if (await GetUser(username) is not null)
            {
                return false;
            }
            
            var user = new User(username, password);
            await _context.Users.AddAsync(user);

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

        public async Task<List<Conversation>> GetUserConversations(int userId)
        {
            var user = await GetUser(userId);
            return user.Conversations;
        } 

        public async Task<User> CreateAndGetUser(string username, string password)
        {
            if (!await CreateUser(username, password))
            {
                throw new InvalidOperationException();
            }
            
            return await GetUser(username);
        }
    }
}