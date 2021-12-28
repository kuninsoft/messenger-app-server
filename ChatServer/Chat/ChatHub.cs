using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServer.Chat.Utilities;
using ChatServer.DataAccess;
using ChatServer.Model;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Chat
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IConversationRepository _conversationDb;
        private readonly IUserRepository _userDb;
        private readonly IMessageRepository _messageDb;
        private readonly IConnectionsManager _connectionsManager;

        public ChatHub(IConversationRepository conversationDb, IUserRepository userDb, IMessageRepository messageDb,
            IConnectionsManager connectionsManager)
        {
            _conversationDb = conversationDb;
            _userDb = userDb;
            _messageDb = messageDb;
            _connectionsManager = connectionsManager;
        }

        public async Task SendPrivateMessage(PrivateMessageRequest request)
        {
            User sender = await _userDb.GetUserByUsername(request.SenderUsername);
            User recipient = await _userDb.GetUserByUsername(request.RecipientUsername);

            if (sender is null)
            {
                await Clients.Caller.FailSendMessage(SendMessageError.SenderUsernameInvalid);
                return;
            }

            if (recipient is null)
            {
                await Clients.Caller.FailSendMessage(SendMessageError.RecipientUsernameInvalid);
            }
            
            string privateConversationName = await GetPrivateConversationName(sender, recipient) ??
                                             await CreatePrivateConversation(sender, recipient);

            await AddMultipleUsersToGroup(privateConversationName, sender, recipient);

            await SendMessageToConversation(new GenericMessage
            {
                SenderUsername = sender.Username,
                ConversationName = privateConversationName,
                Message = request.Message
            });
        }
        
        public async Task SendMessageToConversation(GenericMessage request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                await Clients.Caller.FailSendMessage(SendMessageError.MessageEmpty);
                return;
            }
            
            User sender = await _userDb.GetUserByUsername(request.SenderUsername);
            Conversation conversation = await _conversationDb.GetConversation(request.ConversationName);

            if (sender is null)
            {
                await Clients.Caller.FailSendMessage(SendMessageError.SenderUsernameInvalid);
                return;
            }

            if (conversation is null)
            {
                await Clients.Caller.FailSendMessage(SendMessageError.ConversationInvalid);
                return;
            }


            Message rawMessage = await _messageDb.CreateAndGetMessage(sender, conversation, request.Message, DateTime.Now);

            var messageUnderstandableToClient = new GenericMessage
            {
                SenderUsername = rawMessage.User.Username,
                ConversationName = rawMessage.Conversation.Name,
                ConversationType = rawMessage.Conversation.Type,
                Message = rawMessage.Text,
                Time = rawMessage.Time
            };

            await Clients.Group(conversation.Name).ReceiveMessage(messageUnderstandableToClient);
        }

        private async Task<string> CreatePrivateConversation(User sender, User recipient)
        {
            string privateConversationName = $"{sender.Id}_{recipient.Id}";
            await _conversationDb.CreateConversation(privateConversationName, ConversationType.Private);
            await _conversationDb.AddUsersToConversation(privateConversationName, sender, recipient);
            return privateConversationName;
        }
        
        private async Task<string> GetPrivateConversationName(User first, User second)
        {
            if (await _conversationDb.CheckExistence($"{first.Id}_{second.Id}"))
            {
                return $"{first.Id}_{second.Id}";
            }

            if (await _conversationDb.CheckExistence($"{second.Id}_{first.Id}"))
            {
                return $"{second.Id}_{first.Id}";
            }

            return null;
        }
        
        private async Task InitializeConversationsForConnection()
        {
            User user = _connectionsManager.GetUserByConnection(Context.ConnectionId);
            if (user.Conversations is null) return;

            foreach (Conversation conversation in user.Conversations)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, conversation.Name);
            }
        }

        private async Task AddMultipleUsersToGroup(string group, params User[] users)
        {
            foreach (User user in users)
            {
                HashSet<string> connections = _connectionsManager.GetUserConnections(user);
                if (connections is null) continue;

                foreach (string connection in connections)
                {
                    await Groups.AddToGroupAsync(connection, group);
                }
            }
        }

        #region Obsolete

        [Obsolete] // TODO: Move to the HTTP API
        public async Task GetUserConversations(string username)
        {
            var conversations = _conversationDb.GetConversationsWithUser(username);
            var userConversations = new UserConversations(conversations);
            await Clients.Caller.ReceiveConversationList(userConversations);
        }

        [Obsolete] // TODO: Move to the HTTP API
        public async Task Login(Credentials credentials)
        {
            User user = await _userDb.GetUserByUsername(credentials.Username);
            if (user.CheckPassword(credentials.Password))
            {
                _connectionsManager.MapConnectionToUser(user, Context.ConnectionId);
                await InitializeConversationsForConnection();
                await Clients.Caller.ApproveLogin(credentials.Username);
            }
            else
            {
                await Clients.Caller.FailLogin();
            }
        }

        
        [Obsolete] // TODO: Move to the HTTP API
        public async Task Register(Credentials credentials)
        {
            if (await _userDb.CreateUser(credentials.Username, credentials.Password))
            {
                // For the sense of usability we automatically log user in after registration.
                await Login(credentials);
            }
            else
            {
                await Clients.Caller.FailRegistration();
            }
        }

        [Obsolete] // TODO: Move to the HTTP API
        public async Task GetConversationMessageHistory(string conversation)
        {
            var messages = (await _conversationDb.GetConversation(conversation)).Messages;
            var messageHistory = new ConversationMessageHistory(conversation, messages);
            await Clients.Caller.ReceiveConversationMessageHistory(messageHistory);
        }

        #endregion
    }
}