using Microsoft.VisualStudio.TestTools.UnitTesting;
using YTDownloader.API.Domain.Entities;
using YTDownloader.API.Domain.Abstract;
using YoutubeExplode;
using Microsoft.AspNetCore.Hosting;

namespace YTDownloader.Tests
{
    [TestClass]
    public class YoutubeClientHelperUnitTests
    {
        private static IYoutubeClient client = new YoutubeClient();
        private IYoutubeClientHelper helper= new YoutubeClientHelper(client, "..//YTDownloader.API//wwwroot//ffmpeg.exe");

        [TestMethod]
        public void canGetYoutubeID()
        {
            //Arrange
            string[] testLinks = {
                "https://www.youtube.com/watch?v=Bey4XXJAqS8&t=303s",
                "https://www.youtube.com/watch?v=HmZKgaHa3Fg",
                "https://m.youtube.com/watch?v=2MpUj-Aua48",
                "https://youtu.be/LXb3EKWsInQ",
            };

            string[] wrongLinks =
            {
                "www.google.com",
                "abc",
                "https://www.youtube.com",
                "https://www.youtube.com/watch?v",
                "https://youtu.be/LXb3EKWsI" //non existing video
            };
            //Act
            string[] videoIds = new string[testLinks.Length];
            for(int i=0;i<testLinks.Length;i++)
                videoIds[i] = helper.GetVideoID(testLinks[i]);

            bool[] wasExceptionThrowed = new bool[wrongLinks.Length];
            for(int i=0;i<wrongLinks.Length;i++)
            {
                try
                {
                    helper.GetVideoID(wrongLinks[i]);
                }
                catch(System.FormatException)
                {
                    wasExceptionThrowed[i] = true;
                }
            }

            //Assert
            Assert.AreEqual("Bey4XXJAqS8", videoIds[0]);
            Assert.AreEqual("HmZKgaHa3Fg", videoIds[1]);
            Assert.AreEqual("2MpUj-Aua48", videoIds[2]);
            Assert.AreEqual("LXb3EKWsInQ", videoIds[3]);

            foreach (bool b in wasExceptionThrowed)
                Assert.IsTrue(b);
        }
    }
}
