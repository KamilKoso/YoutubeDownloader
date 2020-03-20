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

namespace YTDownloader.Tests
{

    public class YoutubeClientHelperTests
    {
        const string ffmpegPath = "..//..//..//..//YTDownloader.API//wwwroot//ffmpeg.exe"; //This should be ./YTDownloader/YTDownloader.API/wwwroot/ffmpeg.exe. If ffmpeg.exe is in other location change this string.
        private static readonly IYoutubeClient client = new YoutubeClient();
        private readonly IYoutubeClientHelper target = new YoutubeClientHelper(client, ffmpegPath);


        [Theory]
        [InlineData("https://www.youtube.com/watch?v=Bey4XXJAqS8&t=303s", "https://www.youtube.com/watch?v=HmZKgaHa3Fg", "https://m.youtube.com/watch?v=2MpUj-Aua48", "https://youtu.be/LXb3EKWsInQ")]
        public void GetVideoID_CanGetVideoID(params string[] videoUrls)
        {

            //Act
            string[] videoIds = new string[videoUrls.Length];
            for (int i = 0; i < videoUrls.Length; i++)
                videoIds[i] = target.GetVideoID(videoUrls[i]);

            //Assert
            Assert.Equal("Bey4XXJAqS8", videoIds[0]);
            Assert.Equal("HmZKgaHa3Fg", videoIds[1]);
            Assert.Equal("2MpUj-Aua48", videoIds[2]);
            Assert.Equal("LXb3EKWsInQ", videoIds[3]);
        }

        [Theory]
        [InlineData("www.google.com")]
        [InlineData("abc")]
        [InlineData("https://www.youtube.com")]
        [InlineData("https://www.youtube.com/watch?v")]
        [InlineData("https://youtu.be/LXb3EKWsI")] //non existing video
        public void GetVideoID_ShouldThrowException(string videoUrl)
        {
            Assert.Throws<FormatException>(()=>target.GetVideoID(videoUrl));
        }

        [Theory]
        [InlineData("Bey4XXJAqS8", "HmZKgaHa3Fg")]
        public async Task GetVideoMetadata_CanExtractMetadata(params string[] videoIds)
        {
            //Arrange
            VideoDetails[] results = new VideoDetails[videoIds.Length];
            //Act
            for (int i = 0; i < videoIds.Length; i++)
            {
                results[i] = await target.GetVideoMetadata(videoIds[i]);
            }

            //Assert
            Assert.Equal("4K VIDEO ultrahd hdr sony 4K VIDEOS demo test nature relaxation movie for 4k oled tv", results[0].Title);
            Assert.Equal("LoungeV Films - Relaxing Music and Nature Sounds", results[1].ChannelName);


            List<string> actualQualities = results[0].qualities.ToList();
            List<string> expectedQualities = new List<string>() { "144p", "240p", "360p", "480p", "720p", "1080p", "1440p", "2160p" };
            Assert.True(Enumerable.SequenceEqual(actualQualities.OrderBy(x => x), expectedQualities.OrderBy(x => x)));
        }

        
        [Fact]
        public void Ffmpeg_CheckIfFfmpegExists()
        {
            //If this test fails, DownloadVideo test will fail too. Make sure ffmpeg is in correct location
            //Default correct location is in ./YTDownloader/YTDownloader.API/wwwrooot/ffmpeg.exe
            Assert.True(File.Exists(ffmpegPath));
        }


        //The more InlineData attributes and the higher quality, the more time test will take. It its pretty time consuming test.
        [Theory]
        [InlineData("Bey4XXJAqS8", "..//..//..//TestMediaDir")]
        [InlineData("HmZKgaHa3Fg", "..//..//..//TestMediaDir")]
        public async Task DownloadVideo_CanDownloadVideo(string id, string videoPath, string quality = "144p")
        {
            string videoPathAndName = videoPath + $"//{id}.mp4";
            await target.DownloadVideo(id, quality, videoPathAndName);
            Assert.True(File.Exists(videoPathAndName));
            await CleanDirectory.DeleteFile(videoPath, id + ".mp4");
        }

        [Theory]
        [InlineData("Bey4XXJAqS8", "..//..//..//TestMediaDir")]
        [InlineData("HmZKgaHa3Fg", "..//..//..//TestMediaDir")]
        public async Task DownloadAudio_CanDownloadVideo(string id, string videoPath)
        {
            string videoPathAndName = videoPath + $"//{id}.mp3";
            await target.DownloadAudio(id, videoPathAndName);
            Assert.True(File.Exists(videoPathAndName));
            await CleanDirectory.DeleteFile(videoPath, id + ".mp3");
        }
    }
}
