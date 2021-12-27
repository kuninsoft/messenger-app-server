using System;
using System.Threading.Tasks;

namespace ChatServer.Chat.Utilities
{
    public interface IChatClient
    {
        public Task ReceiveMessage(GenericMessage message);

        #region Obsolete

        [Obsolete] public Task ApproveLogin(string username); 
        [Obsolete] public Task FailLogin();
        [Obsolete] public Task FailRegistration();
        [Obsolete] public Task ReceiveConversationList(UserConversations conversations);
        [Obsolete] public Task ReceiveConversationMessageHistory(ConversationMessageHistory history);

        #endregion
        
    }
}