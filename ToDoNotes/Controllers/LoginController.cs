using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

        private const int keySize = 64;
        private const int iterations = 350000;
        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

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

            if(user.Result != null) 
            {
                var token = Generate(user.Result);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private async Task<User?> Authenticate(UserLogin userLogin)
        {
            var currentUser = await userRepository.GetByUsernameAsync(userLogin.Username);

            if (currentUser == null)
                return null;

            var salt = GetUserSalt(userLogin.Username);

            var checkPass = verifyPassword(userLogin.Password, currentUser.Password, salt);

            if (checkPass)
                return currentUser;
            return null;
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

        private bool verifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

        private byte[]? GetUserSalt(string username)
        {
            var fileName = "./salts.json";
            var listOfUsersSalt = JsonConvert.DeserializeObject<List<UserSalt>>(System.IO.File.ReadAllText(fileName));

            foreach (var userSalt in listOfUsersSalt)
            {
                if (userSalt.Username.Equals(username))
                {
                    byte[] saltBytes = Convert.FromBase64String(userSalt.Salt);
                    return saltBytes;
                }
            }

            return null;
        }
    }
}
