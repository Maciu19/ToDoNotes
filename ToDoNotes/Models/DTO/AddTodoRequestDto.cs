using System.ComponentModel.DataAnnotations;

namespace ToDoNotes.Models.DTO
{
    public class AddTodoRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Title has to be a minimum of 1 character")]
        [MaxLength(500, ErrorMessage = "Title has to be a maximum of 500 characters")]
        public string Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Content has to be a maximum of 1000 characters")]
        public string? Content { get; set; }

        [Required]
        public Guid WorkspaceId { get; set; }
    }
}
