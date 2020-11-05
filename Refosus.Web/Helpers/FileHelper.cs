using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public class FileHelper : IFileHelper
    {
        public async Task<string> UploadFileAsync(IFormFile File, string folder)
        {
            string guid = Guid.NewGuid().ToString();

            string ext = Path.GetExtension(File.FileName);
            string file = $"{guid}" + ext;
            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"wwwroot\\Message\\{folder}",
                file);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }
            return $"~/Message/{folder}/{file}";
        }

    }
}
