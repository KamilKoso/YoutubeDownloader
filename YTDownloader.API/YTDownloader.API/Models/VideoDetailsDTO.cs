using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode.Models;

namespace YTDownloader.API.Models
{
    public class VideoDetails
    {
        public string id { get; set; }
        public string ChannelName { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> qualities { get; set; }
        public ThumbnailSet thumbnails { get; set; }
    }
}
