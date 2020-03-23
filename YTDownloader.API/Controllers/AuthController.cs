using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

            if (await repository.UserExistsAsync(userForRegisterDto.Username))
                return BadRequest("Login already taken !");
            if (await repository.EmailExistsAsync(userForRegisterDto.Email))
                return BadRequest("E-mail already in use");

            User userToCreate = new User()
            {
                Username = userForRegisterDto.Username,
                EmailAddress = userForRegisterDto.Email,
                UserAccountLevel = AccountLevel.Standard,
            
            };
            var createdUser = await repository.RegisterAsync(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDto)
        {
            User userFromRepo = await repository.LoginAsync(userForLoginDto.UsernameOrEmail.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized("Provided login or password is incorrect");

            JwtTokenGenerator tokenGenerator = new JwtTokenGenerator();
            string key = config.GetSection("AppSettings:TokenKey").Value;
            DateTime tokenExpiration = DateTime.Now.AddHours(12);

           
            return Ok(new{token = tokenGenerator.GenerateToken(userFromRepo.Id, userFromRepo.Username, key, tokenExpiration)});
        }
        
        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangeAccountLevel(string username, AccountLevel level)
        {
          bool isSucessfull= await repository.ChangeAccountLevel(username, level);
            if (isSucessfull)
                return Ok("Account level changed !");
            else
                return BadRequest("Something went wrong ! Try again");
        }

    }
}
