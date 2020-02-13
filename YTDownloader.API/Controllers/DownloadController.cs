using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YTDownloader.API.Models;
using Microsoft.AspNetCore.Hosting;
using YTDownloader.API.Domain.Entities;
using YTDownloader.API.Domain.Abstract;


namespace YTDownloader.API.Controllers
{
    //http://localhost:63219/
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly IYoutubeClientHelper clientHelper;
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
            VideoDetails details;
            try 
            { 
             details= await clientHelper.GetVideoMetadata(clientHelper.GetVideoID(videoUrl));
            }
            catch(FormatException)
            { 
                return BadRequest("Provided URL is incorrect");
            }

            return Ok(details);
        }


        //Saves video in the wwwroot/DownloadedVideo dir and returns it .
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetVideo(string id, string quality)
        {
            
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

#pragma warning disable 4014
            //Fire and forget method that will delete the file after 60 seconds. That's not the best solution but i haven't came up with a better
            //one yet.
            CleanDirectory.DeleteFileAfterTime(60, videoDir, id + ".mp4").ConfigureAwait(false);    
#pragma warning restore 4014

            return PhysicalFile(videoPath, "video/mp4", id);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetAudio(string id)
        {
          
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

#pragma warning disable 4014
            //Fire and forget method that will delete the file after 30 seconds. That's not the best solution but i haven't came up with a better
            //one yet.
            CleanDirectory.DeleteFileAfterTime(30, audioDir, id + ".mp3").ConfigureAwait(false);
#pragma warning restore 4014

            return PhysicalFile(audioPath, "audio/mp3", id);
        }
    }
}
