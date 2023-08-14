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
    public class ToDoController : ControllerBase
    {
        private readonly ITodoRepository todoRepository;
        private readonly IMapper mapper;

        public ToDoController(ITodoRepository todoRepository,IMapper mapper)
        {
            this.todoRepository = todoRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todosDomain = await todoRepository.GetAllAsync();

            var todosDto = mapper.Map<List<ToDoDto>>(todosDomain);

            return Ok(todosDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var todoDomain = await todoRepository.GetByIdAsync(id);

            if (todoDomain == null)
                return NotFound();

            var todoDto = mapper.Map<ToDoDto>(todoDomain);

            return Ok(todoDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddTodoRequestDto addTodoRequestDto)
        {
            var todoDomainModel = mapper.Map<ToDo>(addTodoRequestDto);

            todoDomainModel = await todoRepository.CreateAsync(todoDomainModel);

            if(todoDomainModel == null)
                return StatusCode(404, "Workspace dosen't exists.");

            var todoDto = mapper.Map<ToDoDto>(todoDomainModel);

            return Ok(todoDto);
        }

        [HttpPatch]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTodoRequestDto updateTodoRequestDto)
        {
            var existingTodo = await todoRepository.GetByIdAsync(id);
            if (existingTodo == null)
                return NotFound();

            updateTodoRequestDto.Title = updateTodoRequestDto.Title ?? existingTodo.Title;
            updateTodoRequestDto.Content = updateTodoRequestDto.Content ?? existingTodo.Content;

            var todoDomainModel = mapper.Map<ToDo>(updateTodoRequestDto);

            todoDomainModel = await todoRepository.UpdateAsync(id, todoDomainModel);

            var todoDto = mapper.Map<ToDoDto>(todoDomainModel);

            return Ok(todoDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var todoDomainModel = await todoRepository.DeleteAsync(id);

            if (todoDomainModel == null)
                return NotFound();

            var todoDto = mapper.Map<ToDoDto>(todoDomainModel);

            return Ok(todoDto);
        }
    }
}
