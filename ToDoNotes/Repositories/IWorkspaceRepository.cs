using ToDoNotes.Models.Domain;

namespace ToDoNotes.Repositories
{
    public interface IWorkspaceRepository
    {
        Task<List<Workspace>> GetAllAsync();
        Task<Workspace?> GetByIdAsync(Guid id);
        Task<Workspace?> CreateAsync(Workspace workspace);
        Task<Workspace?> UpdateAsync(Guid id, Workspace workspace);
        Task<Workspace?> DeleteAsync(Guid id);
    }
}
