using System.ComponentModel.DataAnnotations;
using ToDoNotes.Models.Domain;

namespace ToDoNotes.Models.DTO
{
    public class AddNoteRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Title has to be a minimum of 1 character")]
        [MaxLength(500, ErrorMessage = "Title has to be a maximum of 500 characters")]
        public string Title { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Content has to be a minimum of 1 character")]
        [MaxLength(1000, ErrorMessage = "Content has to be a maximum of 1000 characters")]
        public string Content { get; set; }

        [Required]
        public Guid WorkspaceId { get; set; }

    }
}
