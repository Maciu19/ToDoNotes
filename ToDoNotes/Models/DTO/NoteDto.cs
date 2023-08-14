﻿using ToDoNotes.Models.Domain;

namespace ToDoNotes.Models.DTO
{
    public class NoteDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public Workspace Workspace { get; set; }
    }
}
