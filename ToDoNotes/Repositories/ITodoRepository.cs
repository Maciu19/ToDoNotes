using ToDoNotes.Models.Domain;

namespace ToDoNotes.Repositories
{
    public interface ITodoRepository
    {
        Task<List<ToDo>> GetAllAsync();

        Task<ToDo?> GetByIdAsync(Guid id);

        Task<ToDo?> CreateAsync(ToDo todo);

        Task<ToDo?> UpdateAsync(Guid id, ToDo todo);

        Task<ToDo?> DeleteAsync(Guid id);
    }
}
