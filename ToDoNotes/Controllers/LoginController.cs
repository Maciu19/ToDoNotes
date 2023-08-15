using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoNotes.Models.Domain;
using ToDoNotes.Models.DTO;
using ToDoNotes.Repositories;

namespace ToDoNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration config;

        public LoginController(IUserRepository userRepository, IConfiguration config)
        {
            this.userRepository = userRepository;
            this.config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if(user != null) 
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");

        }

        private User Authenticate(UserLogin userLogin)
        {
            var currentUser = userRepository.GetByUsernamePassword(userLogin);

            if (currentUser == null)
                return null;

            return currentUser.Result;
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credientals = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credientals
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
