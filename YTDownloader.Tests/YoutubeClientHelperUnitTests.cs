using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Domain.Entities;
using YTDownloader.API.Models;

namespace YTDownloader.Tests
{
    [TestClass]
    public class YoutubeClientHelperUnitTests
    {
        private static readonly IYoutubeClient client = new YoutubeClient();
        private readonly IYoutubeClientHelper helper= new YoutubeClientHelper(client, "..//YTDownloader.API//wwwroot//ffmpeg.exe");

        [TestMethod]
        public void CanGetYoutubeID()
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

        [TestMethod]
        public async Task CanGetVideoMetadata()
        {
            //Arrange
            string[] testIds = {
                "Bey4XXJAqS8",
                "HmZKgaHa3Fg",
                "2MpUj-Aua48",
            };
            VideoDetails[] results = new VideoDetails[testIds.Length];
            //Act
            for(int i=0;i<testIds.Length;i++)
            {
                results[i]= await helper.GetVideoMetadata(testIds[i]);
            }

            //Assert
            Assert.AreEqual("4K VIDEO ultrahd hdr sony 4K VIDEOS demo test nature relaxation movie for 4k oled tv", results[0].Title);
            Assert.AreEqual("LoungeV Films - Relaxing Music and Nature Sounds", results[1].ChannelName);


            List<string> actualQualities = results[2].qualities.ToList(); //VideoURL: https://m.youtube.com/watch?v=2MpUj-Aua48
            List<string> expectedQualities = new List<string>() {"144p", "240p", "360p", "480p", "720p", "1080p", "1440p", "2160p" };
            Assert.IsTrue(Enumerable.SequenceEqual(actualQualities.OrderBy(x => x), expectedQualities.OrderBy(x => x)));
        }

    }
}
