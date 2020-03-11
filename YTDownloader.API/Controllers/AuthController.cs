using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YTDownloader.API.Domain.Abstract;
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

        public AuthController(IAuthRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            userForRegisterDto.Email = userForRegisterDto.Email.ToLower();

            if (await repository.UserExists(userForRegisterDto.Username))
                return BadRequest("User already exists !");

            User userToCreate = new User()
            {
                Username = userForRegisterDto.Username,
                EmailAddress = userForRegisterDto.Email,
                UserAccountLevel = AccountLevel.None,
            
            };
            var createdUser = await repository.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        

    }
}
