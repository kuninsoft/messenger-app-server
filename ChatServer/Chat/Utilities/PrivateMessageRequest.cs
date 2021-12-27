namespace ChatServer.Chat.Utilities
{
    public class PrivateMessageRequest
    {
        public string SenderUsername { get; set; }
        public string RecipientUsername { get; set; }
        public string Message { get; set; }
    }
}