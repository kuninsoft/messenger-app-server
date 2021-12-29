using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatServer.Model
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ConversationType Type { get; set; }
        
        public virtual List<User> Users { get; set; } = new();
        public virtual List<Message> Messages { get; set; } = new();

        public Conversation() { }

        public Conversation(string name, ConversationType type)
        {
            Name = name;
            Type = type;
        }
    }
}