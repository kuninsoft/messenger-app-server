namespace ChatServer.Model
{
    public class MessageAttachment : IEntity
    {
        public int Id { get; set; }
        
        public string Url { get; set; }
        
        public int MessageId { get; set; }
        public virtual Message Message { get; set; }
    }
}