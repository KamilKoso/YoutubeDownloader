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
        private readonly IUserRepository userRepo;
        public AccountPermissionChecker(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        /// <summary>
        /// We want to allow only registred users(Standard level) to download in 1080p resolution, and only Gold level to download in higher resolutions. Account level none should be treaded as unregistered user so can download in up to 720p quality
        /// </summary>
        /// <param name="quality">quality that user wants to download</param>
        /// <param name="isAuthenticated">We check if User.Identity.IsAuthenticated is true</param>
        /// <param name="username">We provide username only if isAuthorized is true</param>
        /// <returns></returns>
        public async Task<(bool isAllowed, string errorMessageIfNotAllowed)> CanDownloadInCertainQualityAsync(string quality, bool isAuthenticated, string username = null)
        {
            string[] splittedQuality = quality.Split('p');   //e.g. 720p60 => splittedQuality[0] = 720, splittedQuality[1] = 60
            int qualityInt = int.Parse(splittedQuality[0]);
            var accountLevel = await userRepo.GetUserAccountLevelAsync(username);

            if (isAuthenticated)
            {
                if (accountLevel == AccountLevel.Gold)
                    return (true, "");

                else if (qualityInt <= 1080)
                {
                    if (accountLevel == AccountLevel.Standard)  //Standard users max quality is 1080p
                        return (true, "");
                }

                else
                    return (false, "You need to have YTGold in order to download in higher qualities !");
                
            }
            else
              if (qualityInt <= 720)            //Not registered users can download only in 720p and lower qualities
                return (true, "");
            else
                return (false, "You need to be logged in !");

            return (false, "Something went wrong !");
        }

    }
}
