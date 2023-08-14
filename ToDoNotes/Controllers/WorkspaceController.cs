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
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceRepository workspaceRepository;
        private readonly IMapper mapper;

        public WorkspaceController(IWorkspaceRepository workspaceRepository, IMapper mapper)
        {
            this.workspaceRepository = workspaceRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var workspacesDomain = await workspaceRepository.GetAllAsync();

            var workspacesDto = mapper.Map<List<WorkspaceDto>>(workspacesDomain);

            return Ok(workspacesDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var workspaceDomain = await workspaceRepository.GetByIdAsync(id);

            if (workspaceDomain == null)
                return NotFound();

            var workspaceDto = mapper.Map<WorkspaceDto>(workspaceDomain);

            return Ok(workspaceDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWorkspaceRequestDto addWorkspaceRequestDto)
        {
            var workspaceDomainModel = mapper.Map<Workspace>(addWorkspaceRequestDto);

            workspaceDomainModel = await workspaceRepository.CreateAsync(workspaceDomainModel);

            var workspaceDto = mapper.Map<WorkspaceDto>(workspaceDomainModel);

            return Ok(workspaceDto);
        }

        [HttpPut] 
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWorkspaceRequestDto updateWorkspaceRequestDto)
        {
            var workspaceDomainModel = mapper.Map<Workspace>(updateWorkspaceRequestDto);

            workspaceDomainModel = await workspaceRepository.UpdateAsync(id, workspaceDomainModel);

            if (workspaceDomainModel == null)
                return NotFound();

            var workspaceDto = mapper.Map<WorkspaceDto>(workspaceDomainModel);

            return Ok(workspaceDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var workspaceDomainModel = await workspaceRepository.DeleteAsync(id);

            if(workspaceDomainModel == null)
                return NotFound();

            var workspaceDto = mapper.Map<WorkspaceDto>(workspaceDomainModel);

            return Ok(workspaceDto);
        }
    }
}
