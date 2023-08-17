using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using ToDoNotes.Models.Domain;
using ToDoNotes.Models.DTO;
using ToDoNotes.Repositories;

namespace ToDoNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        private const int keySize = 64;
        private const int iterations = 350000;
        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public RegisterController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(keySize);

            var userDomainModel = mapper.Map<User>(userRegister);

            userDomainModel.Password = HashPassword(userDomainModel.Password, salt);

            try
            {
                userDomainModel = await userRepository.CreateAsync(userDomainModel);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            SaveUserSalt(userDomainModel.Username, salt);

            var userDto = mapper.Map<UserDto>(userDomainModel);

            return Ok(userDto);
        }

        private string HashPassword(string password, byte[] salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize
            );

            return Convert.ToHexString(hash);
        }

        private void SaveUserSalt(string username, byte[] salt)
        {
            var fileName = "./salts.json";
            var listOfUsersSalt = JsonConvert.DeserializeObject<List<UserSalt>>(System.IO.File.ReadAllText(fileName));

            var userSalt = new UserSalt
            {
                Username = username,
                Salt = Convert.ToHexString(salt)
            };

            if (listOfUsersSalt == null)
            {
                var listOfUsersSaltEmpty = new List<UserSalt>() { userSalt };
                var convertedListOfUsersSaltEmpty = JsonConvert.SerializeObject(listOfUsersSaltEmpty, Formatting.Indented);
                System.IO.File.WriteAllText(fileName, convertedListOfUsersSaltEmpty);
            }
            else
            {
                listOfUsersSalt.Add(userSalt);
                var convertedListOfUsersSalt = JsonConvert.SerializeObject(listOfUsersSalt, Formatting.Indented);
                System.IO.File.WriteAllText(fileName, convertedListOfUsersSalt);
            }
        }
    }
}
