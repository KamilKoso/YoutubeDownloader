using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YTDownloader.API.Models;
using Microsoft.AspNetCore.Hosting;
using YTDownloader.API.Domain.Entities;
using System.IO;
using YTDownloader.API.Domain.Abstract;


namespace YTDownloader.API.Controllers
{
    //http://localhost:63219/
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        private IYoutubeClientHelper clientHelper;
        private readonly IWebHostEnvironment env;
        

        public DownloadController(IYoutubeClientHelper client, IWebHostEnvironment env)
        {
            this.clientHelper = client;
            this.env = env;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetVideoMetaData(string videoUrl)
        {
            VideoDetails details= await clientHelper.GetVideoMetadata(clientHelper.GetVideoID(videoUrl));
            if (details == null)
                return BadRequest("Something went wrong");

            return Ok(details);
        }


        //Saves video in the wwwroot/DownloadedVideo dir and returns it .
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetVideo(string id, string quality)
        {
            VideoStream stream = new VideoStream();
            string videoPath = env.WebRootPath + $"\\DownloadedVideos\\{id}.mp4";
            string videoDir = env.WebRootPath + "\\DownloadedVideos";
             
            try
            {
               await clientHelper.DownloadVideo(id, quality, videoPath);
            }
            catch (ArgumentException)
            {
                return BadRequest(new string("Provided ID is incorrect"));
            }

            MemoryStream videoStream = await stream.prepareVideoStream(videoPath); // No need to dispose MemoryStream, GC will take care of this
            CleanDirectory.DeleteFile(videoDir, id + ".mp4");

            if (videoStream == null)
                return BadRequest();
            return File(videoStream, "video/mp4", id);
            
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetAudio(string id)
        {
            VideoStream stream = new VideoStream();
            string audioPath = env.WebRootPath + $"\\DownloadedVideos\\{id}.mp3";
            string audioDir = env.WebRootPath + "\\DownloadedVideos";

            try
            {
                await clientHelper.DownloadAudio(id, audioPath);
            }
            catch (ArgumentException)
            {
                return BadRequest(new string("Provided ID is incorrect"));
            }

            MemoryStream audioStream = await stream.prepareVideoStream(audioPath); // No need to dispose MemoryStream, GC will take care of this
            CleanDirectory.DeleteFile(audioDir, id + ".mp3");

            if (audioStream == null)
                return BadRequest(new string("Something went wrong ! "));
            return File(audioStream, "audio/mp3", id);
        }
    }
}
