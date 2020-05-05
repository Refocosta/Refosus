using Microsoft.AspNetCore.Mvc;
using Refosus.Web.Data;
using Refosus.Web.Helpers;

namespace Refosus.Web.Controllers
{
    public class CampusController : Controller
    {
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public CampusController(
            DataContext Context,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _context = Context;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context
        //        .Campus
        //        .Include(t=>t.
        //        )
        //}
    }
}
