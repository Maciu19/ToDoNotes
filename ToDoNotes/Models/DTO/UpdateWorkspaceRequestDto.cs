using System.ComponentModel.DataAnnotations;

namespace ToDoNotes.Models.DTO
{
    public class UpdateWorkspaceRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Name has to be a minimum of 1 character")]
        [MaxLength(500, ErrorMessage = "Name has to be a maximum of 500 characters")]
        public string Name { get; set; }
    }
}
