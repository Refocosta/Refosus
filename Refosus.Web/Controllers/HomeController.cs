using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;

namespace Refosus.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger,
            IUserHelper userHelper,
            DataContext context)
        {
            _logger = logger;
            _userHelper = userHelper;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context
                .Menus
                .OrderBy(t => t.Name)
                .ToListAsync());
        }
        public async Task<IActionResult> _MenuAsync()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                var result = await _userHelper.GetUserRolesAsync(user);
            }

            List<string> countries = new List<string>();
            countries.Add("USA");
            countries.Add("UK");
            countries.Add("India");
            return PartialView("_Menu", countries);
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}
