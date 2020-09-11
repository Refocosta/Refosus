using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refosus.Web.Data;
using Refosus.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ShoppingController: Controller
    {
        private readonly DataContext _context;
        private readonly IMailHelper _mailHelper;

        public ShoppingController(DataContext context,IMailHelper mailHelper)
        {
            _context = context;
            _mailHelper = mailHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Info()
        {
            return View();
        }

        public async Task<IActionResult> CreateShopping()
        {
            return View();
        }

        public async Task<IActionResult> DetailsShopping()
        {
            return View();
        }

        public async Task<IActionResult> CreateArticle()
        {
            return View();
        }
        public async Task<IActionResult> DetailsArticleShopping()
        {
            return View();
        }
    }
}
