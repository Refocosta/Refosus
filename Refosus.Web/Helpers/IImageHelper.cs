using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
