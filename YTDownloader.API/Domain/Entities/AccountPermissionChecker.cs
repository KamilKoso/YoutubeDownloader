using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Domain.Concrete;
using YTDownloader.EFDataAccess.Models;

namespace YTDownloader.API.Domain.Entities
{
    public class AccountPermissionChecker : IAccountPermissionChecker
    {
        private readonly UserContext context;
        public AccountPermissionChecker(UserContext context)
        {
            this.context = context;
        }

        public async Task<AccountLevel> CheckAccountLevel(string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user != null)
                return user.UserAccountLevel;
            else
                return AccountLevel.None;
        }
        /// <summary>
        /// We want to allow only registred users(Standard level) to download in 1080p resolution, and only Gold level to download in higher resolutions
        /// </summary>
        /// <param name="quality">quality that user wants to download</param>
        /// <param name="isAuthenticated">We check if User.Identity.IsAuthenticated is true</param>
        /// <param name="username">We provide username only if isAuthorized is true</param>
        /// <returns></returns>
        public async Task<bool> CanDownloadInCertainQuality(string quality, bool isAuthenticated, string username = null)
        {
            string[] splittedQuality = quality.Split('p');   //e.g. 720p60 => splittedQuality[0] = 720, splittedQuality[1] = 60
            int qualityInt = int.Parse(splittedQuality[0]);
            var accountLevel = await CheckAccountLevel(username);

            if (isAuthenticated)
                if (qualityInt <= 1080)
                    if (accountLevel == AccountLevel.Standard)  //Standard users max quality is 1080p
                        return true;
                    else if (accountLevel == AccountLevel.Gold)  //Gold users can download in every quality
                        return true;
            else
              if (qualityInt <= 720)            //Non registered users can download only in 720p and lower qualities
                        return true;

            return false;
        }

    }
}
