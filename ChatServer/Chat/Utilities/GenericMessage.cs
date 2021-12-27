using System;
using ChatServer.Model;

namespace ChatServer.Chat.Utilities
{
    public class GenericMessage
    {
        public string SenderUsername { get; set; }
        public string ConversationName { get; set; }
        public ConversationType ConversationType { get; set; } // Not needed by server, required for client
        public string Message { get; set; }
        public DateTime Time { get; set; } // Not needed by server, required for client
    }
}