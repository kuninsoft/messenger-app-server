using System;
using System.Collections.Generic;
using ChatServer.Model;

namespace ChatServer.Chat.Utilities
{
    public class GenericMessage
    {
        public int SenderId { get; set; }
        public int ConversationId { get; set; }
        public string Message { get; set; }
        public List<int> AttachmentIds { get; set; }
    }

    public class MessageToReceive
    {
        public int Id { get; set; }
        public UserInfo Sender { get; set; }
        public ConversationInfo Conversation { get; set; }
        public string Text { get; set; }
        public List<MessageAttachmentInfo> Attachments;
        public DateTime Time { get; set; }
    }

    public class MessageAttachmentInfo
    {
        public int Id { get; set; }
        public string Url { get; set; }
    }
}