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
        private readonly IConversationDatabase _conversationDb;
        private readonly IUserDatabase _userDb;
        private readonly IMessageDatabase _messageDb;
        private readonly IConnectionsManager _connectionsManager;

        public ChatHub(IConversationDatabase conversationDb, IUserDatabase userDb, IMessageDatabase messageDb,
            IConnectionsManager connectionsManager)
        {
            _conversationDb = conversationDb;
            _userDb = userDb;
            _messageDb = messageDb;
            _connectionsManager = connectionsManager;
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

        public async Task SendPrivateMessage(PrivateMessageRequest request)
        {
            User sender = await _userDb.GetUserByUsername(request.SenderUsername);
            User recipient = await _userDb.GetUserByUsername(request.RecipientUsername);

            // TODO: Inform user of failure
            if (sender is null || recipient is null) return;

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

        [Obsolete] // TODO: Move to the HTTP API
        public async Task GetUserConversations(string username)
        {
            var conversations = _conversationDb.GetConversationsWithUser(username);
            var userConversations = new UserConversations(conversations);
            await Clients.Caller.ReceiveConversationList(userConversations);
        }

        [Obsolete] // TODO: Move to the HTTP API
        public async Task GetConversationMessageHistory(string conversation)
        {
            var messages = (await _conversationDb.GetConversation(conversation)).Messages;
            var messageHistory = new ConversationMessageHistory(conversation, messages);
            await Clients.Caller.ReceiveConversationMessageHistory(messageHistory);
        }
        
        public async Task SendMessageToConversation(GenericMessage request)
        {
            User sender = await _userDb.GetUserByUsername(request.SenderUsername);
            Conversation conversation = await _conversationDb.GetConversation(request.ConversationName);

            // TODO: Inform user of failure
            if (sender is null || conversation is null) return;
            if (string.IsNullOrWhiteSpace(request.Message)) return;

            Message rawMessage = await _messageDb.NewMessage(sender, conversation, request.Message, DateTime.Now);

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
    }
}