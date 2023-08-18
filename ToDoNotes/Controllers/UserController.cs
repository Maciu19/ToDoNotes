using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoNotes.Models.DTO;
using ToDoNotes.Repositories;

namespace ToDoNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userDomain = await userRepository.GetByIdAsync(id);

            if (userDomain == null)
                return NotFound();

            var userDto = mapper.Map<UserDto>(userDomain);

            return Ok(userDto);
        }

        [HttpGet]
        [Route("{username}")]
        [Authorize]
        public async Task<IActionResult> GetByUsername([FromRoute] string username)
        {
            var userDomain = await userRepository.GetByUsernameAsync(username);

            if (userDomain == null)
                return NotFound();

            var userDto = mapper.Map<UserDto>(userDomain);

            return Ok(userDto);
        }
    }
}
