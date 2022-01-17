using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServer.DataAccess;
using ChatServer.Model;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserConversationsController
    {
        private readonly IUserRepository _userRepository;

        public UserConversationsController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Conversation>>> Get(int userId)
        {
            var user = await _userRepository.GetUser(userId);
            
            if (user is not null)
            {
                return user.Conversations;
            }

            return new NotFoundResult();
        }
    }
}