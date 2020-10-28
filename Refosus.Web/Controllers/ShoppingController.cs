using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{

    public class ShoppingController : Controller
    {
        private readonly DataContext _context;
        private readonly IMailHelper _mailHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IUserHelper _userHelper;

        public ShoppingController(DataContext context, IMailHelper mailHelper,
            ICombosHelper combosHelper, IUserHelper userHelper)
        {
            _context = context;
            _mailHelper = mailHelper;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
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
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            List<ShoppingItemsEntity> items = new List<ShoppingItemsEntity>();
            List<ShoppingTempItems> itemsTemp = new List<ShoppingTempItems>();
            ShoppingViewModel model = new ShoppingViewModel
            {

                Users = _combosHelper.GetComboUser(),
                ShoppingUnits = _combosHelper.GetComboShoppingUnit(),
                ShoppingMeasures = _combosHelper.GetComboShoppingMeasure(0),
                Categories = _combosHelper.GetComboShoppingCategory(),
                SubCategories = _combosHelper.GetComboShoppingCategory(0),

                ShoppingStates = _combosHelper.GetComboShoppingState(),

                Projects = _combosHelper.GetComboProject(),





                IdUserCreate = user.Id,
                CreateDate = System.DateTime.Now.ToUniversalTime().ToLocalTime(),
                UpdateDate = System.DateTime.Now.ToUniversalTime().ToLocalTime(),
                IdUserAssign = user.Id,
                IdState = _context.ShoppingStates.FirstOrDefault(s=>s.Name=="Nuevo").Id,
                IdUserProjectBoss = "0",
                Items = items,
                ItemsTemp = itemsTemp
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShopping(ShoppingViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserCreate = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.IdUserCreate);
                model.Project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == model.IdProject);
                model.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Id == model.IdState);
                
                //add Article
                if (model.Operation == 1)
                {
                    //Add Item to temporal Table
                    ShoppingTempItems itm = new ShoppingTempItems
                    {
                        CodSAP = model.CodSap,
                        CreateDate = model.CreateDate,
                        UserCreate = model.UserCreate,
                        Category = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.IdCategory),
                        SubCategory = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.IdSubCategory),
                        Unit = await _context.ShoppingUnits.FirstOrDefaultAsync(u => u.Id == model.IdUnit),
                        Measure = await _context.ShoppingMeasures.FirstOrDefaultAsync(u => u.Id == model.IdMeasure),
                        Quantity = model.Quantity,
                        Description = model.Description,
                        Refence = model.Reference,
                        Mark = model.Mark,
                        InternalOrder = model.InternalOrder,
                        NumInternalOrder = model.NumInternalOrder
                    };
                    _context.Add(itm);
                    await _context.SaveChangesAsync();

                    model.ShoppingUnits = _combosHelper.GetComboShoppingUnit();
                    model.ShoppingMeasures = _combosHelper.GetComboShoppingMeasure(model.IdUnit);
                    model.Categories = _combosHelper.GetComboShoppingCategory();
                    model.SubCategories = _combosHelper.GetComboShoppingCategory(model.IdCategory);
                    model.Users = _combosHelper.GetComboUser();
                    model.ShoppingStates = _combosHelper.GetComboShoppingState();
                    model.Projects = _combosHelper.GetComboProject();
                    model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                        .Where(i => i.UserCreate == itm.UserCreate)
                        .Where(i => i.CreateDate == model.CreateDate)
                        .Include(t => t.Unit)
                        .Include(t => t.Measure);


                    return View(model);
                }
                // Save
                if (model.Operation == 2)
                {
                    return View(model);
                }
                // Create
                if (model.Operation == 3)
                {
                    return View(model);
                }
            }
            model.ShoppingUnits = _combosHelper.GetComboShoppingUnit();
            model.ShoppingMeasures = _combosHelper.GetComboShoppingMeasure(model.IdUnit);
            model.Categories = _combosHelper.GetComboShoppingCategory();
            model.SubCategories = _combosHelper.GetComboShoppingCategory(model.IdCategory);
            model.Users = _combosHelper.GetComboUser();
            model.ShoppingStates = _combosHelper.GetComboShoppingState();
            model.Projects = _combosHelper.GetComboProject();

            model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                        .Where(i => i.UserCreate.Id == model.IdUserCreate)
                        .Where(i => i.CreateDate == model.CreateDate)
                        .Include(t => t.Unit)
                        .Include(t => t.Measure);
            return View(model);
        }







        public async Task<IActionResult> EditShopping()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            List<ShoppingItemsEntity> items = new List<ShoppingItemsEntity>();
            List<ShoppingTempItems> itemsTemp = new List<ShoppingTempItems>();
            ShoppingViewModel model = new ShoppingViewModel
            {

                Users = _combosHelper.GetComboUser(),
                ShoppingUnits = _combosHelper.GetComboShoppingUnit(),
                ShoppingMeasures = _combosHelper.GetComboShoppingMeasure(0),
                Categories = _combosHelper.GetComboShoppingCategory(),
                SubCategories = _combosHelper.GetComboShoppingCategory(0),

                ShoppingStates = _combosHelper.GetComboShoppingState(),

                Projects = _combosHelper.GetComboProject(),





                IdUserCreate = user.Id,
                CreateDate = System.DateTime.Now.ToUniversalTime().ToLocalTime(),
                UpdateDate = System.DateTime.Now.ToUniversalTime().ToLocalTime(),
                IdUserAssign = user.Id,
                IdState = _context.ShoppingStates.FirstOrDefault(s => s.Name == "Nuevo").Id,
                IdUserProjectBoss = "0",
                Items = items,
                ItemsTemp = itemsTemp
            };
            return View(model);
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





        #region ServicesCombo
        public async Task<string> UserProjectBoss(int UBP)
        {
            string cod = _context.Projects.FirstOrDefaultAsync(p => p.Id == UBP).Result.IdUserProjectBoss;
            return cod;
        }

        public async Task<IEnumerable> CombosSubCategory(int val)
        {
            IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> list = _combosHelper.GetComboShoppingCategory(val);
            return list;
        }
        public async Task<IEnumerable> CombosMeasure(int val)
        {
            IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> list = _combosHelper.GetComboShoppingMeasure(val);
            return list;
        }
        #endregion

    }
}
