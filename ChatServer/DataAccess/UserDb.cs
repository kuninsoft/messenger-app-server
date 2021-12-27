using System;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatServer.DataAccess
{
    public class UserDb : IUserDatabase
    {
        public async Task<User> GetUserByUsername(string username)
        {
            await using var context = new AppContext();

            return await context.Users
                .Include(u => u.Conversations)
                .Include(u => u.Messages)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> CreateUser(string username, string password)
        {
            await using var context = new AppContext();

            if (await GetUserByUsername(username) is not null)
            {
                return false;
            }
            
            var user = new User(username, password);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<User> CreateAndGetUser(string username, string password)
        {
            if (!await CreateUser(username, password))
            {
                throw new InvalidOperationException();
            }
            
            return await GetUserByUsername(username);
        }
    }
}