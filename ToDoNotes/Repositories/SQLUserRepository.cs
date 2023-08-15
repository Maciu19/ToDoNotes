using Microsoft.EntityFrameworkCore;
using ToDoNotes.Data;
using ToDoNotes.Models.Domain;
using ToDoNotes.Models.DTO;

namespace ToDoNotes.Repositories
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly ToDoNotesDbContext dbContext;

        public SQLUserRepository(ToDoNotesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User?> GetByUsernamePassword(UserLogin userLogin)
        {
            return await dbContext.User.FirstOrDefaultAsync(u => u.Username == userLogin.Username && u.Password == userLogin.Password);
        }

        public async Task<User?> GetById(Guid id)
        {
            return await dbContext.User.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
