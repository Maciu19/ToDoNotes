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

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await dbContext.User.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await dbContext.User.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> CreateAsync(User user)
        {
            var existingUser = await dbContext.User.FirstOrDefaultAsync(u => u.Username == user.Username);

            if(existingUser != null)
            {
                throw new InvalidOperationException("A user with the same username already exists.");
            }

            await dbContext.User.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return user;
        }
    }
}
