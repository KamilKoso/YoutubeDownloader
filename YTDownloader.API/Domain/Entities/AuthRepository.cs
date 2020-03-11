using System.Threading.Tasks;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Domain.Concrete;
using YTDownloader.API.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace YTDownloader.API.Domain.Entities
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserContext context;

#region public methods

        public AuthRepository(UserContext context)
        {
            this.context = context;
        }

        public async Task<User> Login(string UsernameOrEmail, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == UsernameOrEmail || x.EmailAddress == UsernameOrEmail);

            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            else
                return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHashSalt(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username, string email)
        {
            if (await context.Users.AnyAsync(x => x.Username == username) || await context.Users.AnyAsync(x=> x.EmailAddress == email))
                return true;
            else
                return false;
        }

        #endregion

#region private methods

        private void CreatePasswordHashSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for(int i=0;i<passwordHash.Length;i++)
                    if (computedHash[i] != passwordHash[i])
                        return false;
            }
          return true;
        }
#endregion
    }
}
