using AutoMapper;
using ToDoNotes.Models.Domain;
using ToDoNotes.Models.DTO;

namespace ToDoNotes.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Workspace, WorkspaceDto>().ReverseMap();
            CreateMap<Workspace, AddWorkspaceRequestDto>().ReverseMap();
            CreateMap<Workspace, UpdateWorkspaceRequestDto>().ReverseMap();

            CreateMap<Note, NoteDto>().ReverseMap();   
            CreateMap<Note, AddNoteRequestDto>().ReverseMap();
            CreateMap<Note, UpdateNoteRequestDto>().ReverseMap();

            CreateMap<ToDo, ToDoDto>().ReverseMap();
            CreateMap<ToDo, AddTodoRequestDto>().ReverseMap();
            CreateMap<ToDo, UpdateTodoRequestDto>().ReverseMap();

            CreateMap<User, UserRegister>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
