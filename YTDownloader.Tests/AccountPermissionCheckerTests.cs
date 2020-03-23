using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YoutubeExplode;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Domain.Entities;
using YTDownloader.API.Models;
using Moq;
using YTDownloader.API.Domain.Concrete;
using YTDownloader.EFDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace YTDownloader.Tests
{
    public class AccountPermissionCheckerTests
    {
         string[] NotRegisteredUsersQualities = new string[]
        {
          "144p",
          "240p",
          "360p",
          "480p",
          "720p",
          "144p30 HDR",
          "240p30 HDR",
          "360p30 HDR",
          "480p60 HDR",
          "720p60 HDR",
        };

        string[] StandardUsersQualities = new string[] //Every lower quality + this
        {
          "1080p",
          "1080p60 HDR",
        };

        string[] GoldUsersQualities = new string[] //Every lower quality + this
        {
           "1440p",
           "2160p",
           "1440p60 HDR",
           "2160p60 HDR"
        };


        [Fact]
        public async Task CanDownloadInCertainQuality_NotRegisteredUsers_ShouldBeAbleTo()
        {
            //Arrange
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            AccountPermissionChecker target = new AccountPermissionChecker(mock.Object);
            bool isAuthenticated = false;

            foreach (string quality in NotRegisteredUsersQualities)
            {
                //Act
                var result = await target.CanDownloadInCertainQualityAsync(quality, isAuthenticated);
                bool canDownload = result.isAllowed;
                string errorMessage = result.errorMessageIfNotAllowed;
                    
                //Assert
                Assert.True(canDownload);
                Assert.True(errorMessage == "");
            }
        }

        [Fact]
        public async Task CanDownloadInCertainQuality_NotRegisteredUsers_ShouldNotBeAbleTo()
        {
            //Arrange
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            AccountPermissionChecker target = new AccountPermissionChecker(mock.Object);
            bool isAuthenticated = false;

            foreach (string quality in StandardUsersQualities.Concat(GoldUsersQualities))
            {
                //Act
                var result = await target.CanDownloadInCertainQualityAsync(quality, isAuthenticated);
                bool canDownload = result.isAllowed;
                string errorMessage = result.errorMessageIfNotAllowed;

                //Assert
                Assert.False(canDownload);
                Assert.True(errorMessage == "You need to be logged in !");
            }
        }

       [Fact]
        public async Task CanDownloadInCertainQuality_StandardUsers_ShouldBeAbleTo()
        {
            //Arrange
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            mock.Setup(x => x.GetUserAccountLevelAsync(It.IsAny<string>())).ReturnsAsync(AccountLevel.Standard);
            AccountPermissionChecker target = new AccountPermissionChecker(mock.Object);
            bool isAuthenticated = true;

            foreach (string quality in NotRegisteredUsersQualities.Concat(StandardUsersQualities))
            {
                //Act
                var result = await target.CanDownloadInCertainQualityAsync(quality, isAuthenticated);
                bool canDownload = result.isAllowed;
                string errorMessage = result.errorMessageIfNotAllowed;

                //Assert
                Assert.True(canDownload);
                Assert.True(errorMessage == "");
            }
        }

       [Fact]
        public async Task CanDownloadInCertainQuality_StandardUsers_ShouldNotBeAbleTo()
        {
            //Arrange
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            mock.Setup(x => x.GetUserAccountLevelAsync(It.IsAny<string>())).ReturnsAsync(AccountLevel.Standard);
            AccountPermissionChecker target = new AccountPermissionChecker(mock.Object);
            bool isAuthenticated = true;

            foreach (string quality in GoldUsersQualities)
            {
                //Act
                var result = await target.CanDownloadInCertainQualityAsync(quality, isAuthenticated);
                bool canDownload = result.isAllowed;
                string errorMessage = result.errorMessageIfNotAllowed;

                //Assert
                Assert.False(canDownload);
                Assert.True(errorMessage == "You need to have YTGold in order to download in higher qualities !");
            }
        }

        [Fact]
        public async Task CanDownloadInCertainQuality_GoldUsers_ShouldBeAbleTo()
        {
            //Arrange
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            mock.Setup(x => x.GetUserAccountLevelAsync(It.IsAny<string>())).ReturnsAsync(AccountLevel.Gold);
            AccountPermissionChecker target = new AccountPermissionChecker(mock.Object);
            bool isAuthenticated = true;

            foreach (string quality in NotRegisteredUsersQualities.Concat(StandardUsersQualities).Concat(GoldUsersQualities))
            {
                //Act
                var result = await target.CanDownloadInCertainQualityAsync(quality, isAuthenticated);
                bool canDownload = result.isAllowed;
                string errorMessage = result.errorMessageIfNotAllowed;

                //Assert
                Assert.True(canDownload);
                Assert.True(errorMessage == "");
            }
        }
    }
}
