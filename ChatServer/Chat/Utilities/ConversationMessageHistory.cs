using System.Collections.Generic;
using ChatServer.Model;

namespace ChatServer.Chat.Utilities
{
    public class ConversationMessageHistory
    {
        public string ConversationName { get; set; }
        public List<GenericMessage> Messages { get; set; }

        public ConversationMessageHistory(string conversationName, List<Message> messages)
        {
            ConversationName = conversationName;
            Messages = new List<GenericMessage>();
            foreach (Message message in messages)
            {
                Messages.Add(new GenericMessage
                {
                    SenderUsername = message.User.Username,
                    ConversationName = message.Conversation.Name,
                    ConversationType = message.Conversation.Type,
                    Message = message.Text,
                    Time = message.Time
                });
            }
        }
    }
}