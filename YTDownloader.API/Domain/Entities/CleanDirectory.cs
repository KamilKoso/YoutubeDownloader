using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace YTDownloader.API.Domain.Entities
{
    public static class CleanDirectory
    {
        public static void DeleteAllFiles(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach(FileInfo file in directory.EnumerateFiles())
            {
                file.Delete();
            }
        }

        public static void DeleteAllSubDirs(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (DirectoryInfo dir in directory.EnumerateDirectories())
            {
                dir.Delete(true);
            }
        }

        public static void DeleteFile(string path, string fileName)
        {
            FileInfo file = new FileInfo(Path.Combine(path,fileName));
            file.Delete();
        }

        public static void DeleteFile(string pathAndFileName)
        {
            FileInfo file = new FileInfo(pathAndFileName);
            file.Delete();
        }

        public async static Task DeleteFileAfterTime(int timeInSeconds, string path, string fileName)
        {
            await Task.Run(()=> {
                Thread.Sleep(timeInSeconds * 1000);
                DeleteFile(path, fileName);
                }).ConfigureAwait(false);
        }
    }
}
