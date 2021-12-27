using System.Collections.Generic;
using ChatServer.Model;

namespace ChatServer.Chat.Utilities
{
    public class ConversationInfo
    {
        public string Name { get; set; }
        public ConversationType Type { get; set; }
        public List<UserInfo> Users { get; set; }
    }
}