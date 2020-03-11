using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YTDownloader.API.Models;
using Microsoft.AspNetCore.Hosting;
using YTDownloader.API.Domain.Entities;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Custom;


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
             
             
            try
            {
               await clientHelper.DownloadVideo(id, quality, videoPath);
            }
            catch (ArgumentException)
            {
                return BadRequest(new string("Provided ID is incorrect"));
            }

            return new PhysicalFileResultAndDelete(videoPath, "video/mp4");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetAudio(string id)
        {
          
            string audioPath = env.WebRootPath + $"\\DownloadedVideos\\{id}.mp3";

            try
            {
                await clientHelper.DownloadAudio(id, audioPath);
            }
            catch (ArgumentException)
            {
                return BadRequest(new string("Provided ID is incorrect"));
            }

            return new PhysicalFileResultAndDelete(audioPath, "audio/mp3");
        }
    }
}
