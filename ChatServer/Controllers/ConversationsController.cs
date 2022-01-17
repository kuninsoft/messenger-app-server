using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServer.DataAccess;
using ChatServer.Model;
using Microsoft.AspNetCore.Mvc;


namespace ChatServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : Controller
    {
        private readonly IConversationRepository _conversationRepository;

        public ConversationsController(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        [HttpGet("[id]")]
        public async Task<ActionResult<Conversation>> Get(int id)
        {
            return await _conversationRepository.GetConversation(id);
        }
    }
}