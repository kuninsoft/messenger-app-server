using System.Threading.Tasks;
using ChatServer.Model;

namespace ChatServer.DataAccess
{
    public interface IUserDatabase
    {
        public Task<User> GetUserByUsername(string username);
        public Task<bool> CreateUser(string username, string password);
        public Task<User> CreateAndGetUser(string username, string password);
    }
}