using System.ComponentModel;

namespace ToDoNotes.Models.Domain
{
    public class Workspace
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }    
        public User User { get; set; }  
    }
}
