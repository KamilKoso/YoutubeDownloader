using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YTDownloader.API.Models
{
    public class VideoDetailsDTO
    {
        public string id { get; set; }
        public IEnumerable<string> qualities { get; set; }
    }
}
