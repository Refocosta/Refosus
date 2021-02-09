using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System;
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
        private readonly IConverterHelper _converterHelper;
        public ShoppingController(DataContext context, IMailHelper mailHelper,
            ICombosHelper combosHelper, IUserHelper userHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _mailHelper = mailHelper;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }
        public async Task<IActionResult> CreateShopping()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            DateTime now = System.DateTime.Now.ToUniversalTime().ToLocalTime();
            List<ShoppingItemsEntity> items = new List<ShoppingItemsEntity>();
            List<ShoppingTempItems> itemsTemp = new List<ShoppingTempItems>();
            ShoppingViewModel model = new ShoppingViewModel
            {
                IdUserCreate = user.Id,
                CreateDate = now,
                UpdateDate = now,
                IdUserAssign = user.Id,
                IdState = _context.ShoppingStates.FirstOrDefault(s => s.Name == "Nuevo").Id,
                IdUserProjectBoss = "0",
                Items = items,
                ItemsTemp = itemsTemp
            };
            model = await returnModelAsync(model);
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
                ShoppingEntity entity = await _converterHelper.ToShoppingEntityAsync(model, true);
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
                    model = await returnModelAsync(model);
                    return View(model);
                }
                // Save
                if (model.Operation == 2)
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Guardado");
                    entity.UpdateDate = System.DateTime.Now.ToUniversalTime().ToLocalTime();
                    model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                .Where(i => i.UserCreate == model.UserCreate)
                .Where(i => i.CreateDate == model.CreateDate)
                .Include(t => t.Unit)
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                .Include(t => t.Measure);
                    foreach (ShoppingTempItems item in model.ItemsTemp)
                    {
                        ShoppingItemsEntity objItem = new ShoppingItemsEntity
                        {
                            CodSAP = item.CodSAP,
                            Category = item.Category,
                            SubCategory = item.SubCategory,
                            Unit = item.Unit,
                            Measure = item.Measure,
                            Quantity = item.Quantity,
                            Description = item.Description,
                            Refence = item.Refence,
                            Mark = item.Mark,
                            InternalOrder = item.InternalOrder,
                            NumInternalOrder = item.NumInternalOrder,
                            Shoping = entity
                        };
                        _context.Add(objItem);
                        _context.Remove(item);
                    }
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                    model = await returnModelAsync(model);
                    return RedirectToAction(nameof(DetailsShopping), new { entity.Id });
                }
                // Create
                if (model.Operation == 3)
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "En Cotización");
                    entity.UpdateDate = System.DateTime.Now.ToUniversalTime().ToLocalTime();
                    //ENVIAR A COMPRAS
                    //ENVIAR A COMPRAS
                    //ENVIAR A COMPRAS
                    //ENVIAR A COMPRAS
                    //ENVIAR A COMPRAS
                    //ENVIAR A COMPRAS
                    //ENVIAR A COMPRAS
                    //ENVIAR A COMPRAS
                    //ENVIAR A COMPRAS
                    model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                .Where(i => i.UserCreate == model.UserCreate)
                .Where(i => i.CreateDate == model.CreateDate)
                .Include(t => t.Unit)
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                .Include(t => t.Measure);
                    foreach (ShoppingTempItems item in model.ItemsTemp)
                    {
                        ShoppingItemsEntity objItem = new ShoppingItemsEntity
                        {
                            CodSAP = item.CodSAP,
                            Category = item.Category,
                            SubCategory = item.SubCategory,
                            Unit = item.Unit,
                            Measure = item.Measure,
                            Quantity = item.Quantity,
                            Description = item.Description,
                            Refence = item.Refence,
                            Mark = item.Mark,
                            InternalOrder = item.InternalOrder,
                            NumInternalOrder = item.NumInternalOrder,
                            Shoping = entity
                        };
                        _context.Add(objItem);
                        _context.Remove(item);
                    }
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                    model = await returnModelAsync(model);
                    return RedirectToAction(nameof(DetailsShopping), new { entity.Id });
                }
                // EditarArticulo
                if (model.Operation == 4)
                {
                    model = await returnModelAsync(model);
                    return View(model);
                }
                // Eliminar Articulo Temp
                if (model.Operation == 5)
                {

                    ShoppingTempItems ItemTemp = await _context.ShoppingTempItems
                .FirstOrDefaultAsync(m => m.Id == model.DeleteItem);
                    _context.Remove(ItemTemp);
                    await _context.SaveChangesAsync();
                    model = await returnModelAsync(model);
                    return View(model);
                }
            }
            model = await returnModelAsync(model);
            return View(model);
        }
        public async Task<IActionResult> DetailsShopping(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ShoppingEntity shoppingEntity = await _context.Shoppings
                .Include(t => t.UserCreate)
                .Include(t => t.State)
                .Include(t => t.Project)
                .Include(t => t.Items)
                .Include(t => t.Items)
                .ThenInclude(t => t.Category)
                .Include(t => t.Items)
                .ThenInclude(t => t.SubCategory)
                .Include(t => t.Items)
                .ThenInclude(t => t.Unit)
                .Include(t => t.Items)
                .ThenInclude(i => i.Measure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingEntity == null)
            {
                return NotFound();
            }
            return View(shoppingEntity);
        }
        public async Task<IActionResult> EditShopping(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<ShoppingTempItems> itemsTemp = new List<ShoppingTempItems>();
            ShoppingEntity entity = await _context.Shoppings
                .Include(t => t.UserCreate)
                .Include(t => t.State)
                .Include(t => t.Project)
                .Include(t => t.Items)
                .Include(t => t.Items)
                .ThenInclude(t => t.Category)
                .Include(t => t.Items)
                .ThenInclude(t => t.SubCategory)
                .Include(t => t.Items)
                .ThenInclude(t => t.Unit)
                .Include(t => t.Items)
                .ThenInclude(i => i.Measure)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            ShoppingViewModel model = await _converterHelper.ToShoppingViewModelAsync(entity);

            model.Items = _context.ShoppingItems.Select(i => i)
                .Where(i => i.Shoping == entity)
                .Include(t => t.Unit)
                .Include(t => t.Measure)
                .Include(t => t.Category)
                .Include(t => t.SubCategory);
            model.ItemsTemp = itemsTemp;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditShopping(ShoppingViewModel model)
        {
            model.UserCreate = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.IdUserCreate);
            model.Project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == model.IdProject);
            model.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Id == model.IdState);
            ShoppingEntity entity = await _converterHelper.ToShoppingEntityAsync(model, false);
            model.Items = _context.ShoppingItems.Where(i => i.Shoping == entity);

            //Guardar
            if (model.Operation == 1)
            {
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            }
            //Enviar Cotizacion
            if (model.Operation == 2)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Por Autorización");
            }
            //Autorizar
            if (model.Operation == 3)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Autorizado");
            }
            //Rechazar
            if (model.Operation == 4)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Rechazado");
            }
            //Entrega Parcial
            if (model.Operation == 5)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "En Entrega");
            }
            //Entrega Completa
            if (model.Operation == 6)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Entregado");
            }
            //Cancelar
            if (model.Operation == 7)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Cancelado");
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsShopping), new { entity.Id });
            }
            //Devolver
            if (model.Operation == 8)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Devuelto");
                entity.UserAssigned = entity.UserCreate;
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsShopping), new { entity.Id });
            }
            //Enviar a Compras
            if (model.Operation == 9)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "En Cotización");
                //ENVIAR A COMPRAS
                //ENVIAR A COMPRAS
                //ENVIAR A COMPRAS
                //ENVIAR A COMPRAS
                //ENVIAR A COMPRAS
                //ENVIAR A COMPRAS
                //ENVIAR A COMPRAS
                //ENVIAR A COMPRAS
                //ENVIAR A COMPRAS
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsShopping), new { entity.Id });
            }
            if (model.Operation == 10)
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
                
                model = await returnModelAsync(model);
                return View(model);
            }
                model = await returnModelAsync(model);
            return View(model);
        }







        public async Task<IActionResult> DeleteArticleTemp(int? id)
        {
            if (id == null)
            {
                NotFound();
            }
            ShoppingTempItems entity = await _context.ShoppingTempItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            _context.Remove(entity);
            await _context.SaveChangesAsync();

            ShoppingViewModel model = new ShoppingViewModel();


            model = await returnModelAsync(model);
            return View(model);
        }









        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Info()
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

        #region Helper
        public async Task<ShoppingViewModel> returnModelAsync(ShoppingViewModel model)
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            model.ShoppingUnits = _combosHelper.GetComboShoppingUnit();
            model.ShoppingMeasures = _combosHelper.GetComboShoppingMeasure(model.IdUnit);
            model.Categories = _combosHelper.GetComboShoppingCategory();
            model.SubCategories = _combosHelper.GetComboShoppingCategory(model.IdCategory);
            model.Users = _combosHelper.GetComboUser();
            model.ShoppingStates = _combosHelper.GetComboShoppingState();
            model.Projects = _combosHelper.GetComboProject();
            model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                .Where(i => i.UserCreate == user)
                .Where(i => i.CreateDate == model.CreateDate)
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                .Include(t => t.Unit)
                .Include(t => t.Measure);
            return (model);
        }
        #endregion

    }
}
