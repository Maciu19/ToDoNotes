using ToDoNotes.Models.Domain;
using ToDoNotes.Models.DTO;

namespace ToDoNotes.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernamePassword(UserLogin userLogin);
        Task<User?> GetById(Guid id);
        Task<User> CreateAsync(User user);
    }
}
