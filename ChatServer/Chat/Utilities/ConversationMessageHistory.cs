using System.Collections.Generic;
using ChatServer.Model;

namespace ChatServer.Chat.Utilities
{
    public class ConversationMessageHistory
    {
        public string ConversationName { get; set; }
        public List<MessageToReceive> Messages { get; set; }

        public ConversationMessageHistory(string conversationName, List<Message> messages)
        {
            ConversationName = conversationName;
            Messages = new List<MessageToReceive>();
            foreach (Message message in messages)
            {
                Messages.Add(
                        new MessageToReceive
                        {
                            Id = message.Id
                        }
                    );
            }
        }
    }
}