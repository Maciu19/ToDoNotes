using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ToDoNotes.Data;
using ToDoNotes.Models.Domain;

namespace ToDoNotes.Repositories
{
    public class SQLWorkspaceRepository : IWorkspaceRepository
    {
        private readonly ToDoNotesDbContext dbContext;

        public SQLWorkspaceRepository(ToDoNotesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Workspace>> GetAllAsync()
        {
            return await dbContext.Workspace
                .Include("User")
                .ToListAsync();
        }

        public async Task<Workspace?> GetByIdAsync(Guid id)
        {
            return await dbContext.Workspace
                .Include("User")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Workspace?> CreateAsync(Workspace workspace)
        {
            var existingUser = await dbContext.User.FirstOrDefaultAsync(x => x.Id == workspace.UserId);

            if (existingUser == null)
                throw new InvalidOperationException("User dosen't exists.");

            await dbContext.Workspace.AddAsync(workspace);
            await dbContext.SaveChangesAsync();
            return workspace;
        }

        public async Task<Workspace?> UpdateAsync(Guid id, Workspace workspace)
        {
            var existingWorkspace = await dbContext.Workspace.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWorkspace == null)
                return null;

            existingWorkspace.Name = workspace.Name;

            await dbContext.SaveChangesAsync();

            return existingWorkspace;
        }

        public async Task<Workspace?> DeleteAsync(Guid id)
        {
            var existingWorkspace = await dbContext.Workspace.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWorkspace == null)
                return null;

            dbContext.Workspace.Remove(existingWorkspace);
            await dbContext.SaveChangesAsync();

            return existingWorkspace;
        }
    }
}
