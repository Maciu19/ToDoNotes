using System.ComponentModel.DataAnnotations;

namespace ToDoNotes.Models.DTO
{
    public class UpdateTodoRequestDto
    {
        [MaxLength(500, ErrorMessage = "Title has to be a maximum of 500 characters")]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Content has to be a maximum of 1000 characters")]
        public string? Content { get; set; }
    }
}
