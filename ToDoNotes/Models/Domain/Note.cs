namespace ToDoNotes.Models.Domain
{
    public class Note
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid WorkspaceId { get; set; }
        public Workspace Workspace { get; set; } 
    }
}
