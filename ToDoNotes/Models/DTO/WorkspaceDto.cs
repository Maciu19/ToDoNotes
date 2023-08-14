using ToDoNotes.Models.Domain;

namespace ToDoNotes.Models.DTO
{
    public class WorkspaceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
