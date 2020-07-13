using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface IFileHelper
    {
        Task<string> UploadFileAsync(IFormFile File, string folder);
    }
}
