using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoutubeExplode.Converter;
using YoutubeExplode;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Models.MediaStreams;
using YTDownloader.API.Models;
using Microsoft.AspNetCore.Hosting;
using YTDownloader.API.Infrastructure;
using System.IO;


namespace YTDownloader.API.Controllers
{
    //http://localhost:63219/
    [ApiController]
    [Route("[controller]")]
    public class VideoDownloadController : ControllerBase
    {
        private IYoutubeClient client;
        private IYoutubeConverter converter;
        private IWebHostEnvironment env;
        

        public VideoDownloadController(IYoutubeClient client, IWebHostEnvironment env)
        {
            this.client = client;
            this.converter = new YoutubeConverter(client, env.WebRootPath + "\\ffmpeg.exe");
            this.env = env;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetVideoMetaData(string videoUrl)
        {
            string id = YoutubeClient.ParseVideoId(videoUrl);
            var video = await client.GetVideoAsync(id);
            MediaStreamInfoSet streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);
            IEnumerable<string> qualities = streamInfoSet.GetAllVideoQualityLabels();
            return Ok(new VideoDetails{id=id,ChannelName=video.Author,Title=video.Title, qualities=qualities, thumbnails=video.Thumbnails });
        }


        //Saves video in the wwwroot/DownloadedVideo dir sends it .
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetVideo(string id, string quality)
        {
            MediaStreamInfoSet streamInfoSet;
            string videoPath = env.WebRootPath + $"\\DownloadedVideos\\{id}.mp4";
             
            try
            {
                streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);
                var audioStreamInfo = streamInfoSet.Audio.WithHighestBitrate();
                var videoStreamInfo = streamInfoSet.Video.FirstOrDefault(c => c.VideoQualityLabel == quality);
                var mediaStreamInfos = new MediaStreamInfo[] { audioStreamInfo, videoStreamInfo };
                await converter.DownloadAndProcessMediaStreamsAsync(mediaStreamInfos, videoPath, "mp4");
            }
            catch (ArgumentException)
            {
                return BadRequest(new string("Provided ID is incorrect"));
            }

            VideoStream stream = new VideoStream();
            var video = await client.GetVideoAsync(id);

            MemoryStream videoStream = await stream.prepareVideoStream(videoPath); // No need to dispose MemoryStream, GC will take care of this
            if (videoStream == null)
                    return BadRequest();

                CleanDirectory.DeleteFile(Path.Combine(env.WebRootPath,"//DownloadedVideos"), id + ".mp4");
                return File(videoStream, "video/mp4", video.Title);
            
        }

        
    }
}
