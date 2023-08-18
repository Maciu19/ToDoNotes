using Microsoft.EntityFrameworkCore;
using ToDoNotes.Data;
using ToDoNotes.Models.Domain;

namespace ToDoNotes.Repositories
{
    public class SQLNoteRepository : INoteRepository
    {
        private readonly ToDoNotesDbContext dbContext;

        public SQLNoteRepository(ToDoNotesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Note>> GetAllAsync()
        {
            return await dbContext.Note
                .Include("Workspace")
                .ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(Guid id)
        {
            return await dbContext.Note
                .Include("Workspace")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Note?> CreateAsync(Note note)
        {
            var existingWorkspace = await dbContext.Workspace.FirstOrDefaultAsync(x => x.Id == note.WorkspaceId);

            if (existingWorkspace == null)
                throw new InvalidOperationException("Workspace dosen't exists.");

            await dbContext.Note.AddAsync(note);    
            await dbContext.SaveChangesAsync(); 
            return note;    
        }

        public async Task<Note?> UpdateAsync(Guid id, Note note)
        {
            var existingNote = await dbContext.Note.FirstOrDefaultAsync(x => x.Id == id);

            existingNote.Title = note.Title;
            existingNote.Content = note.Content;

            await dbContext.SaveChangesAsync();
            
            return existingNote;
        }

        public async Task<Note?> DeleteAsync(Guid id)
        {
            var existingNote = await dbContext.Note.FirstOrDefaultAsync(x => x.Id == id);
            if (existingNote == null)
                return null;

            dbContext.Note.Remove(existingNote); 
            await dbContext.SaveChangesAsync();
            return existingNote;
        }
    }
}
