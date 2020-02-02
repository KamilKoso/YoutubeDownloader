using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YTDownloader.API.Domain.Entities
{
    public class VideoStream
    {
        public async Task<MemoryStream> PrepareVideoStream(string videoPath)
        {
            if (!File.Exists(videoPath))
                return null;
            var memory = new MemoryStream(); // No need to dispose MemoryStream, GC will take care of this

                using (var stream = new FileStream(videoPath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return memory;
        }
    }
}
