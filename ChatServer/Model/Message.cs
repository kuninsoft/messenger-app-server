using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatServer.Model
{
    public class Message : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime Time { get; set; }
        
        public int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<MessageAttachment> Attachments { get; set; }
        
        public Message() { }

        public Message(string text, DateTime time)
        {
            Text = text;
            Time = time;
        }
    }
}