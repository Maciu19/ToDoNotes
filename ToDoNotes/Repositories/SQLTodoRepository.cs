using Microsoft.EntityFrameworkCore;
using ToDoNotes.Data;
using ToDoNotes.Models.Domain;

namespace ToDoNotes.Repositories
{
    public class SQLTodoRepository : ITodoRepository
    {
        private readonly ToDoNotesDbContext dbContext;

        public SQLTodoRepository(ToDoNotesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ToDo>> GetAllAsync()
        {
            return await dbContext.ToDo
                .Include("Workspace")
                .ToListAsync(); 
        }

        public async Task<ToDo?> GetByIdAsync(Guid id)
        {
            return await dbContext.ToDo
                .Include("Workspace")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ToDo?> CreateAsync(ToDo todo)
        {
            var existingWorkspace = await dbContext.Workspace.FirstOrDefaultAsync(x => x.Id == todo.WorkspaceId);
            if (existingWorkspace == null)
                return null;

            await dbContext.ToDo.AddAsync(todo);
            await dbContext.SaveChangesAsync();

            return todo;    

        }

        public async Task<ToDo?> UpdateAsync(Guid id, ToDo todo)
        {
            var existingToDo = await dbContext.ToDo.FirstOrDefaultAsync(x => x.Id == id);

            existingToDo.Title = todo.Title;
            existingToDo.Content = todo.Content;

            await dbContext.SaveChangesAsync();

            return existingToDo;
        }

        public async Task<ToDo?> DeleteAsync(Guid id)
        {
            var existingToDo = await dbContext.ToDo.FirstOrDefaultAsync(x => x.Id == id);
            if (existingToDo == null)
                return null;

            dbContext.ToDo.Remove(existingToDo);
            await dbContext.SaveChangesAsync();
            return existingToDo;
        }
    }
}
