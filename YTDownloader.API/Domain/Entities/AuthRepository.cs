using System.Threading.Tasks;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Domain.Concrete;
using YTDownloader.API.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using YTDownloader.EFDataAccess.Models;

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

        public async Task<User> LoginAsync(string usernameOrEmail, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == usernameOrEmail || x.EmailAddress == usernameOrEmail);

            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            else
                return user;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHashSalt(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            if (await context.Users.AnyAsync(x => x.Username == username))
                return true;
            else
                return false;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            if (await context.Users.AnyAsync(x => x.EmailAddress == email))
                return true;
            else
                return false;
        }

        public async Task<bool> ChangeAccountLevel(string username, AccountLevel accountLevel)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if(user!=null)
            {
                user.UserAccountLevel = accountLevel;
                context.Users.Update(user);
                await context.SaveChangesAsync();
                return true;
            }
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

                for (int i = 0; i < passwordHash.Length; i++)
                    if (computedHash[i] != passwordHash[i])
                        return false;
            }
            return true;
        }
        #endregion
    }
}
