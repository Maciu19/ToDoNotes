using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoNotes.CustomActionFilters;
using ToDoNotes.Models.Domain;
using ToDoNotes.Models.DTO;
using ToDoNotes.Repositories;

namespace ToDoNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteRepository noteRepository;
        private readonly IMapper mapper;

        public NoteController(INoteRepository noteRepository, IMapper mapper)
        {
            this.noteRepository = noteRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notesDomain = await noteRepository.GetAllAsync();

            var notesDto = mapper.Map<List<NoteDto>>(notesDomain);

            return Ok(notesDto);    
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var noteDomain = await noteRepository.GetByIdAsync(id);

            if (noteDomain == null)
                return NotFound();

            var noteDto = mapper.Map<NoteDto>(noteDomain);

            return Ok(noteDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddNoteRequestDto addNoteRequestDto)
        {
            var noteDomainModel = mapper.Map<Note>(addNoteRequestDto);

            noteDomainModel = await noteRepository.CreateAsync(noteDomainModel);

            if (noteDomainModel == null)
                return NotFound("Workspace dosen't exists.");

            var noteDto = mapper.Map<NoteDto>(noteDomainModel);

            return Ok(noteDto);
        }

        [HttpPatch]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateNoteRequestDto updateNoteRequestDto)
        {
            var existingNote = await noteRepository.GetByIdAsync(id);
            if (existingNote == null)
                return NotFound();

            updateNoteRequestDto.Title = updateNoteRequestDto.Title ?? existingNote.Title;
            updateNoteRequestDto.Content = updateNoteRequestDto.Content ?? existingNote.Content;

            var noteDomainModel = mapper.Map<Note>(updateNoteRequestDto);

            noteDomainModel = await noteRepository.UpdateAsync(id, noteDomainModel);

            var noteDto = mapper.Map<NoteDto>(noteDomainModel);

            return Ok(noteDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var noteDomainModel = await noteRepository.DeleteAsync(id);

            if (noteDomainModel == null)
                return NotFound();

            var noteDto = mapper.Map<NoteDto>(noteDomainModel);

            return Ok(noteDto);
        }
    }
}
