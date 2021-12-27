using System.Collections.Generic;
using ChatServer.Model;

namespace ChatServer.Chat.Utilities
{
    public class UserConversations
    {
        public List<ConversationInfo> Conversations { get; set; }

        public UserConversations(List<Conversation> conversations)
        {
            Conversations = new();
            
            foreach (Conversation conversation in conversations)
            {
                var convInfo = new ConversationInfo
                {
                    Name = conversation.Name,
                    Type = conversation.Type,
                    Users = new List<UserInfo>()
                };

                foreach (User user in conversation.Users)
                {
                    convInfo.Users.Add(new UserInfo
                    {
                        Username = user.Username
                    });
                }
                
                Conversations.Add(convInfo);
            }
        }
    }
}