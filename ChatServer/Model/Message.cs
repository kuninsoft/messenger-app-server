using System;
using System.ComponentModel.DataAnnotations;

namespace ChatServer.Model
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime Time { get; set; }
        
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }

        public Message() { }

        public Message(string text, DateTime time)
        {
            Text = text;
            Time = time;
        }
    }
}