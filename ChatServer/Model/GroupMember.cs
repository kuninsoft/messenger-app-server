using System.ComponentModel.DataAnnotations;

namespace ChatServer.Model
{
    public class GroupMember : IEntity
    {
        [Key]
        public int Id { get; set; }
        
        public UserRole Role { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }
    }
}