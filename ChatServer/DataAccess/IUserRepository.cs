using System.Threading.Tasks;
using ChatServer.Model;

namespace ChatServer.DataAccess
{
    public interface IUserRepository
    {
        public Task<User> GetUser(string username);
        public Task<User> GetUser(int id);
        public Task<bool> CreateUser(string username, string password);
        public Task<User> CreateAndGetUser(string username, string password);
    }
}