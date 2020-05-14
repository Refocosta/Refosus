using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ISecurityHelper _securityHelper;

        public HomeController(ILogger<HomeController> logger,
            IUserHelper userHelper,
            DataContext context,
            ISecurityHelper securityHelper)
        {
            _logger = logger;
            _userHelper = userHelper;
            _context = context;
            _securityHelper = securityHelper;
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
                List<RoleMenuEntity> menus = await _securityHelper.GetMenusRoleAsync(user);
                return PartialView("_menu", menus);
            }
            return NotFound();
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
