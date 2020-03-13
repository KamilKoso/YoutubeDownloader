using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Domain.Entities;
using YTDownloader.API.Models;
using YTDownloader.EFDataAccess.Models;

namespace YTDownloader.API.Controllers
{
    //http://localhost:63219/
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repository;
        public IConfiguration config;

        public AuthController(IAuthRepository repository, IConfiguration config)
        {
            this.repository = repository;
            this.config = config;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDto)
        { 
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            userForRegisterDto.Email = userForRegisterDto.Email.ToLower();

            if (await repository.UserExists(userForRegisterDto.Username, userForRegisterDto.Email))
                return BadRequest("User already exists !");

            User userToCreate = new User()
            {
                Username = userForRegisterDto.Username,
                EmailAddress = userForRegisterDto.Email,
                UserAccountLevel = AccountLevel.Standard,
            
            };
            var createdUser = await repository.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDto)
        {
            User userFromRepo = await repository.Login(userForLoginDto.UsernameOrEmail.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized("Provided login or password is incorrect");

            JwtTokenGenerator tokenGenerator = new JwtTokenGenerator();
            string key = config.GetSection("AppSettings:TokenKey").Value;
            DateTime tokenExpiration = DateTime.Now.AddHours(12);

            string token = tokenGenerator.GenerateToken(userFromRepo.Id, userFromRepo.Username, key, tokenExpiration);
           
            return Ok(token);
        }
        

    }
}
