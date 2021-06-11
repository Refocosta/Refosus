using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
namespace Refosus.Web.Controllers
{
    public class ShoppingController : Controller
    {
        //ENVIO A 1:CATEGORIA 2:SUBCATEGORIA 3GRUPO COMPRAS
        private readonly int envio = 1;
        private readonly DataContext _context;
        private readonly IMailHelper _mailHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IFileHelper _fileHelper;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        public ShoppingController(DataContext context, IMailHelper mailHelper,
            ICombosHelper combosHelper, IUserHelper userHelper, IFileHelper fileHelper,
            IConverterHelper converterHelper, IConfiguration configuration)
        {
            _context = context;
            _mailHelper = mailHelper;
            _combosHelper = combosHelper;
            _fileHelper = fileHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _configuration = configuration;
        }
        #region Index
        [Authorize(Roles = "Administrator,ShoppingAdmin")]
        public async Task<IActionResult> Index()
        {
            return View(await
                    _context.Shoppings
                    .Include(t => t.UserCreate)
                    .Include(t => t.State)
                    .Include(t => t.Project)
                    .Include(t => t.UserProjectBoss)
                    .Include(t => t.AssignedGroup)
                    .Include(t => t.UserAssigned)
                .ToListAsync());
        }
        [Authorize(Roles = "Administrator,ShoppingGroup")]
        public async Task<IActionResult> IndexCompras()
        {
            return View(await
                    _context.Shoppings
                    .Include(t => t.UserCreate)
                    .Include(t => t.State)
                    .Include(t => t.Project)
                    .Include(t => t.AssignedGroup)
                    .Include(t => t.UserAssigned)
                    .Where(t => t.AssignedGroup.Name == "Compras")
                    .Where(t => (t.State.Name == "En Cotización" || t.State.Name == "En Compra" || t.State.Name == "Autorizado" || t.State.Name == "En Entrega"))
                .ToListAsync());
        }
        [Authorize(Roles = "Administrator,ShoppingCategory")]
        public async Task<IActionResult> IndexCategoria()
        {
            List<ShoppingEntity> lista = await
                    _context.Shoppings
                    .Include(t => t.UserCreate)
                    .Include(t => t.State)
                    .Include(t => t.Project)
                    .Include(t => t.AssignedGroup)
                    .Include(t => t.UserAssigned)
                    .Include(t => t.Items)
                    .Include(t => t.Items)
                    .ThenInclude(t => t.Category)
                    .ThenInclude(t => t.Responsable)
                    .Where(t => t.AssignedGroup.Name == "Compras")
                    .Where(t => (t.State.Name == "En Cotización" || t.State.Name == "En Compra" || t.State.Name == "Autorizado" || t.State.Name == "En Entrega"))
                .ToListAsync();
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            List<ShoppingEntity> temp = new List<ShoppingEntity>();
            try
            {
                foreach (ShoppingEntity reg in lista)
                {
                    if (reg.Items.Count() > 0 && reg.Items != null)
                    {
                        List<ShoppingItemsEntity> items = reg.Items.ToList();
                        try
                        {
                            foreach (ShoppingItemsEntity item in items)
                            {
                                if (item.Category.Responsable != user)
                                {
                                    items.Remove(item);
                                }
                            }
                        }
                        catch { }
                        reg.Items = items;
                        if (reg.Items.Count() == 0)
                        {
                            //lista.Remove(reg);
                        }
                        else
                        {
                            temp.Add(reg);
                        }
                    }
                    else
                    {
                        //lista.Remove(reg);
                    }
                }
            }
            catch { }
            return View(temp);
        }
        [Authorize(Roles = "Administrator,ShoppingPending")]
        public async Task<IActionResult> IndexPending()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            return View(await
                    _context.Shoppings
                    .Include(t => t.UserCreate)
                    .Include(t => t.State)
                    .Include(t => t.Project)
                    .Include(t => t.AssignedGroup)
                    .Include(t => t.UserAssigned)
                    .Include(t => t.UserCreate)
                    .Where(t => t.UserAssigned == user)
                    .Where(t => (t.State.Name != "Rechazado" && t.State.Name != "Cancelado" && t.State.Name != "Finalizado"))
                .ToListAsync());
        }
        [Authorize(Roles = "Administrator,ShoppingMe")]
        public async Task<IActionResult> IndexMe()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            return View(await
                    _context.Shoppings
                    .Include(t => t.UserCreate)
                    .Include(t => t.State)
                    .Include(t => t.Project)
                    .Include(t => t.AssignedGroup)
                    .Include(t => t.UserAssigned)
                    .Include(t => t.UserCreate)
                    .Where(t => t.UserCreate == user)
                    .Where(t => (t.State.Name != "Rechazado" && t.State.Name != "Cancelado" && t.State.Name != "Finalizado"))
                .ToListAsync());
        }
        [Authorize(Roles = "Administrator,ShoppingMeHistory")]
        public async Task<IActionResult> IndexMeHistory()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            return View(await
                    _context.Shoppings
                    .Include(t => t.UserCreate)
                    .Include(t => t.State)
                    .Include(t => t.Project)
                    .Include(t => t.AssignedGroup)
                    .Include(t => t.UserAssigned)
                    .Include(t => t.UserCreate)
                    .Where(t => t.UserCreate == user)
                    .Where(t => (t.State.Name == "Rechazado" || t.State.Name == "Cancelado" || t.State.Name == "Finalizado"))
                .ToListAsync());
        }
        #endregion
        #region Create
        [Authorize(Roles = "Administrator,ShoppingCreator")]
        public async Task<IActionResult> CreateShopping(int? id)
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            DateTime now = System.DateTime.Now.ToUniversalTime();
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
            model.Groups = _combosHelper.GetGroupsUser(user.Id);
            if (model.Groups.Count() > 0)
            {
                model.IdGroupCreate = int.Parse(model.Groups.ToList()[0].Value);
            }
            model.Pantalla = (int)id;
            return View(model);
        }
        [Authorize(Roles = "Administrator,ShoppingCreator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShopping(ShoppingViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                model.UserAssigned = await _userHelper.GetUserByIdAsync(model.IdUserAssign);
                model.Company = await _context.Companies.FirstOrDefaultAsync(u => u.Id == model.IdCompany);
                model.UserCreate = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.IdUserCreate);
                model.Project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == model.IdProject);
                model.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Id == model.IdState);
                model.CreateGroup = await _context.TP_Groups.FirstOrDefaultAsync(s => s.Id == model.IdGroupCreate);
                ShoppingEntity entity = await _converterHelper.ToShoppingEntityAsync(model, true);
                string id = "";
                //add Article
                if (model.Operation == 1)
                {
                    //Add Item to temporal Table
                    ShoppingTempItems itm = new ShoppingTempItems
                    {
                        CodSAP = model.ItemCodSap,
                        Description = model.ItemDescription,
                        CreateDate = model.CreateDate,
                        UpdateDate = model.UpdateDate,
                        UserCreate = model.UserCreate,
                        Category = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.ItemIdCategory),
                        SubCategory = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.ItemIdSubCategory),
                        Unit = await _context.ShoppingUnits.FirstOrDefaultAsync(u => u.Id == model.ItemIdUnit),
                        Measure = await _context.ShoppingMeasures.FirstOrDefaultAsync(u => u.Id == model.ItemIdMeasure),
                        Quantity = model.ItemQuantity,
                        Reference = model.ItemReference,
                        Mark = model.ItemMark,
                        InternalOrder = model.ItemInternalOrder,
                        NumInternalOrder = model.ItemNumInternalOrder,
                        Observation = model.ItemObservation
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
                    entity.AssignedGroup = entity.CreateGroup;
                    model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                .Where(i => i.UserCreate == model.UserCreate)
                .Where(i => i.CreateDate == model.CreateDate)
                .Where(i => i.UpdateDate == model.UpdateDate)
                .Include(t => t.Unit)
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                .Include(t => t.Measure);
                    foreach (ShoppingTempItems item in model.ItemsTemp)
                    {
                        ShoppingItemsEntity objItem = new ShoppingItemsEntity
                        {
                            Shoping = entity,
                            CodSAP = item.CodSAP,
                            Description = item.Description,
                            Category = item.Category,
                            SubCategory = item.SubCategory,
                            Unit = item.Unit,
                            Measure = item.Measure,
                            Quantity = item.Quantity,
                            Reference = item.Reference,
                            Mark = item.Mark,
                            InternalOrder = item.InternalOrder,
                            NumInternalOrder = item.NumInternalOrder,
                            Observation = item.Observation,
                            QuantityDelivered = 0,
                            UserAssigned = null,
                            State = await _context.TP_Shopping_Item_State.SingleOrDefaultAsync(o => o.Nombre == "Nuevo")
                        };
                        if (envio == 1)
                        {
                            objItem.UserAssigned = await _userHelper.GetUserByIdAsync(_context.ShoppingCategories.Include(o => o.Responsable).SingleAsync(o => o.Id == objItem.Category.Id).Result.Responsable.Id);
                        }
                        else
                        if (envio == 2)
                        {
                            objItem.UserAssigned = await _userHelper.GetUserByIdAsync(_context.ShoppingCategories.Include(o => o.Responsable).SingleAsync(o => o.Id == objItem.SubCategory.Id).Result.Responsable.Id);
                        }
                        else
                        {
                            objItem.UserAssigned = null;
                        }
                        _context.Add(objItem);
                        _context.Remove(item);
                    }
                    entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                    model = await returnModelAsync(model);
                    id = entity.Id + "-" + model.Pantalla;
                    #region EMAIL
                    string Asunto = "Compra  No. " + entity.Id;
                    string Body = "Se ha creado la compra No <strong>" + entity.Id + "</strong>,  " +
                    "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                    if (entity.UserAssigned != null)
                    {
                        Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                    }
                    else
                    {
                        Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                    }
                    List<string> to = new List<string>
                    {
                        entity.UserCreate.Email,
                        user.Email
                    };
                    if (entity.UserAssigned != null)
                    {
                        to.Add(entity.UserAssigned.Email);
                    }
                    else
                    {
                        to.Add(entity.AssignedGroup.Email);
                    }
                    _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                    #endregion
                    return RedirectToAction(nameof(DetailsShopping), new { id });
                }
                // Create
                if (model.Operation == 3)
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "En Cotización");
                    entity.AssignedGroup = await _context.TP_Groups.FirstOrDefaultAsync(g => g.Name == "Compras" && g.Company.Id == entity.Company.Id);
                    entity.UserAssigned = null;
                    model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                .Where(i => i.UserCreate == model.UserCreate)
                .Where(i => i.CreateDate == model.CreateDate)
                .Where(i => i.UpdateDate == model.UpdateDate)
                .Include(t => t.Unit)
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                .Include(t => t.Measure);
                    foreach (ShoppingTempItems item in model.ItemsTemp)
                    {
                        ShoppingItemsEntity objItem = new ShoppingItemsEntity
                        {
                            Shoping = entity,
                            CodSAP = item.CodSAP,
                            Description = item.Description,
                            Category = item.Category,
                            SubCategory = item.SubCategory,
                            Unit = item.Unit,
                            Measure = item.Measure,
                            Quantity = item.Quantity,
                            Reference = item.Reference,
                            Mark = item.Mark,
                            InternalOrder = item.InternalOrder,
                            NumInternalOrder = item.NumInternalOrder,
                            Observation = item.Observation,
                            QuantityDelivered = 0,
                            State = await _context.TP_Shopping_Item_State.SingleOrDefaultAsync(o => o.Nombre == "Nuevo")
                        };
                        if (envio == 1)
                        {
                            objItem.UserAssigned = await _userHelper.GetUserByIdAsync(_context.ShoppingCategories.Include(o => o.Responsable).SingleAsync(o => o.Id == objItem.Category.Id).Result.Responsable.Id);
                        }
                        else
                        if (envio == 2)
                        {
                            objItem.UserAssigned = await _userHelper.GetUserByIdAsync(_context.ShoppingCategories.Include(o => o.Responsable).SingleAsync(o => o.Id == objItem.SubCategory.Id).Result.Responsable.Id);
                        }
                        else
                        {
                            objItem.UserAssigned = null;
                        }
                        _context.Add(objItem);
                        _context.Remove(item);
                    }
                    entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                    model = await returnModelAsync(model);
                    id = entity.Id + "-" + model.Pantalla;
                    #region EMAIL
                    string Asunto = "Compra  No. " + entity.Id;
                    string Body = "Se ha creado la compra No <strong>" + entity.Id + "</strong>,  " +
                    "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                    if (entity.UserAssigned != null)
                    {
                        Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                    }
                    else
                    {
                        Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                    }
                    List<string> to = new List<string>
                    {
                        entity.UserCreate.Email,
                        user.Email
                    };
                    if (entity.UserAssigned != null)
                    {
                        to.Add(entity.UserAssigned.Email);
                    }
                    else
                    {
                        to.Add(entity.AssignedGroup.Email);
                    }
                    _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                    #endregion
                    return RedirectToAction(nameof(DetailsShopping), new { id });
                }
                // EditarArticulo
                if (model.Operation == 4)
                {
                    ShoppingTempItems itm = new ShoppingTempItems
                    {
                        Id = model.EditTempItem,
                        CodSAP = model.ItemCodSap,
                        Description = model.ItemDescription,
                        CreateDate = model.CreateDate,
                        UpdateDate = model.UpdateDate,
                        UserCreate = model.UserCreate,
                        Category = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.ItemIdCategory),
                        SubCategory = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.ItemIdSubCategory),
                        Unit = await _context.ShoppingUnits.FirstOrDefaultAsync(u => u.Id == model.ItemIdUnit),
                        Measure = await _context.ShoppingMeasures.FirstOrDefaultAsync(u => u.Id == model.ItemIdMeasure),
                        Quantity = model.ItemQuantity,
                        Reference = model.ItemReference,
                        Mark = model.ItemMark,
                        InternalOrder = model.ItemInternalOrder,
                        NumInternalOrder = model.ItemNumInternalOrder,
                        Observation = model.ItemObservation
                    };
                    _context.Update(itm);
                    await _context.SaveChangesAsync();
                    model = await returnModelAsync(model);
                    #region EMAIL
                    string Asunto = "Compra  No. " + entity.Id;
                    string Body = "Se ha creado la compra No <strong>" + entity.Id + "</strong>,  " +
                    "Creado por <strong>" + entity.UserCreate.FullName + "</strong> y asignado a <strong>";
                    if (entity.UserAssigned != null)
                    {
                        Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                    }
                    else
                    {
                        Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                    }
                    List<string> to = new List<string>
                    {
                        entity.UserCreate.Email
                    };
                    if (entity.UserAssigned != null)
                    {
                        to.Add(entity.UserAssigned.Email);
                    }
                    else
                    {
                        to.Add(entity.AssignedGroup.Email);
                    }
                    _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                    #endregion
                    return View(model);
                }
                // Eliminar Articulo Temp
                if (model.Operation == 5)
                {
                    ShoppingTempItems ItemTemp = await _context.ShoppingTempItems.FirstOrDefaultAsync(m => m.Id == model.DeleteItem);
                    _context.Remove(ItemTemp);
                    await _context.SaveChangesAsync();
                    model = await returnModelAsync(model);
                    return View(model);
                }
            }
            model = await returnModelAsync(model);
            return View(model);
        }
        #endregion
        #region Detalles
        [Authorize]
        public async Task<IActionResult> DetailsShopping(string id)
        {
            if (id.Split('-').Count() == 2)
            {
                int cod = int.Parse(id.Split('-')[0]);
                if (cod == 0)
                {
                    return NotFound();
                }
                ShoppingEntity entity = await _context.Shoppings
                     .Include(t => t.UserCreate)
                     .Include(t => t.State)
                     .Include(t => t.Project)
                     .Include(t => t.UserProjectBoss)
                     .Include(t => t.Items)
                     .Include(t => t.Items)
                     .ThenInclude(t => t.Category)
                     .Include(t => t.Items)
                     .ThenInclude(t => t.SubCategory)
                     .Include(t => t.Items)
                     .ThenInclude(t => t.Unit)
                     .Include(t => t.Items)
                     .ThenInclude(i => i.Measure)
                     .Include(t => t.Items)
                     .ThenInclude(i => i.State)
                     .Include(t => t.AssignedGroup)
                     .Include(t => t.CreateGroup)
                     .Include(t => t.Company)
                     .Include(t => t.UserAssigned)
                    .FirstOrDefaultAsync(m => m.Id == cod);
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
                    .Include(t => t.SubCategory)
                    .Include(t => t.UserAssigned);
                model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                    .Where(i => i.UserCreate.Id == model.IdUserCreate)
                    .Where(i => i.CreateDate == model.CreateDate)
                    .Include(t => t.Category)
                    .Include(t => t.SubCategory)
                    .Include(t => t.Unit)
                    .Include(t => t.Measure);
                foreach (ShoppingTempItems item in model.ItemsTemp)
                {
                    _context.Remove(item);
                }
                await _context.SaveChangesAsync();
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                model = await returnModelAsync(model);
                model.Groups = _combosHelper.GetGroupsUser(user.Id);
                model.Pantalla = int.Parse(id.Split('-')[1]);
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }
        [Authorize]
        public async Task<IActionResult> DetailsShoppingCategoria(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            ShoppingEntity shoppingEntity = await _context.Shoppings
                .Include(t => t.UserCreate)
                .Include(t => t.State)
                .Include(t => t.Project)
                .Include(t => t.Items)
                .Include(t => t.Items)
                .ThenInclude(t => t.Category)
                .Include(t => t.Items)
                .ThenInclude(t => t.Category)
                .ThenInclude(t => t.Responsable)
                .Include(t => t.Items)
                .ThenInclude(t => t.SubCategory)
                .Include(t => t.Items)
                .ThenInclude(t => t.Unit)
                .Include(t => t.Items)
                .ThenInclude(i => i.Measure)
                .Include(t => t.AssignedGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingEntity == null)
            {
                return NotFound();
            }
            List<ShoppingItemsEntity> obj = new List<ShoppingItemsEntity>();
            try
            {
                foreach (ShoppingItemsEntity item in shoppingEntity.Items)
                {
                    if (item.Category.Responsable == user)
                    {
                        obj.Add(item);
                    }
                }
            }
            catch { }
            shoppingEntity.Items = obj;
            return View(shoppingEntity);
        }
        #endregion
        #region Edit
        public async Task<IActionResult> EditShopping(string id)
        {
            if (id.Split('-').Count() == 2)
            {
                int cod = int.Parse(id.Split('-')[0]);
                if (cod == 0)
                {
                    return NotFound();
                }
                ShoppingEntity entity = await _context.Shoppings
                .Include(t => t.UserCreate)
                .Include(t => t.State)
                .Include(t => t.Project)
                .Include(t => t.UserProjectBoss)
                .Include(t => t.Items)
                .Include(t => t.Items)
                .ThenInclude(t => t.Category)
                .Include(t => t.Items)
                .ThenInclude(t => t.SubCategory)
                .Include(t => t.Items)
                .ThenInclude(t => t.Unit)
                .Include(t => t.Items)
                .ThenInclude(i => i.Measure)
                .Include(t => t.Items)
                .ThenInclude(i => i.State)
                .Include(t => t.AssignedGroup)
                .Include(t => t.CreateGroup)
                .Include(t => t.Company)
                .Include(t => t.UserAssigned)
                .FirstOrDefaultAsync(s => s.Id == cod);
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
                    .Include(t => t.SubCategory)
                    .Include(t => t.UserAssigned);

                model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                    .Where(i => i.UserCreate.Id == model.IdUserCreate)
                    .Where(i => i.CreateDate == model.CreateDate)
                    .Include(t => t.Category)
                    .Include(t => t.SubCategory)
                    .Include(t => t.Unit)
                    .Include(t => t.Measure);
                foreach (ShoppingTempItems item in model.ItemsTemp)
                {
                    _context.Remove(item);
                }
                await _context.SaveChangesAsync();
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                model = await returnModelAsync(model);
                model.Groups = _combosHelper.GetGroupsUser(user.Id);
                model.Pantalla = int.Parse(id.Split('-')[1]);
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditShopping(ShoppingViewModel model)
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            string Id = "";
            model.Company = await _context.Companies.FirstOrDefaultAsync(u => u.Id == model.IdCompany);
            model.UserCreate = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.IdUserCreate);
            model.Project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == model.IdProject);
            model.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Id == model.IdState);
            model.AssignedGroup = await _context.TP_Groups.FirstOrDefaultAsync(s => s.Id == model.IdGroupAssigned);
            model.UserAssigned = await _userHelper.GetUserByIdAsync(model.IdUserAssign);
            model.CreateGroup = await _context.TP_Groups.FirstOrDefaultAsync(s => s.Id == model.IdGroupCreate);
            ShoppingEntity entity = await _converterHelper.ToShoppingEntityAsync(model, false);
            model.Items = _context.ShoppingItems
                .Where(i => i.Shoping == entity)
                .Include(t => t.Unit)
                .Include(t => t.Measure)
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                .Include(t => t.State)
                .Include(t => t.UserAssigned);
            //Guardar
            if (model.Operation == 1)
            {
                entity.UserAssigned = await _userHelper.GetUserAsync(User.Identity.Name);
                model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
            .Where(i => i.UserCreate == model.UserCreate)
            .Where(i => i.CreateDate == model.CreateDate)
            .Where(i => i.UpdateDate == model.UpdateDate)
            .Include(t => t.Unit)
            .Include(t => t.Category)
            .Include(t => t.SubCategory)
            .Include(t => t.Measure);
                foreach (ShoppingTempItems item in model.ItemsTemp)
                {
                    ShoppingItemsEntity objItem = new ShoppingItemsEntity
                    {
                        Shoping = entity,
                        CodSAP = item.CodSAP,
                        Description = item.Description,
                        Category = item.Category,
                        SubCategory = item.SubCategory,
                        Unit = item.Unit,
                        Measure = item.Measure,
                        Quantity = item.Quantity,
                        Reference = item.Reference,
                        Mark = item.Mark,
                        InternalOrder = item.InternalOrder,
                        NumInternalOrder = item.NumInternalOrder,
                        Observation = item.Observation,
                        QuantityDelivered = 0,
                        UserAssigned = null,
                        State = await _context.TP_Shopping_Item_State.SingleOrDefaultAsync(o => o.Nombre == "Nuevo")
                    };
                    if (envio == 1)
                    {
                        objItem.UserAssigned = await _userHelper.GetUserByIdAsync(_context.ShoppingCategories.Include(o => o.Responsable).SingleAsync(o => o.Id == objItem.Category.Id).Result.Responsable.Id);
                    }
                    else
                    if (envio == 2)
                    {
                        objItem.UserAssigned = await _userHelper.GetUserByIdAsync(_context.ShoppingCategories.Include(o => o.Responsable).SingleAsync(o => o.Id == objItem.SubCategory.Id).Result.Responsable.Id);
                    }
                    else
                    {
                        objItem.UserAssigned = null;
                    }
                    _context.Add(objItem);
                    _context.Remove(item);
                }
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Enviar Cotizacion -- MAIL OK
            if (model.Operation == 2)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Por Autorización");
                entity.AssignedGroup = entity.CreateGroup;
                List<TP_Shopping_Usu_Apr_GroEntity> list = new List<TP_Shopping_Usu_Apr_GroEntity>();
                list = _context.TP_Shopping_Usu_Apr_Gro
                .Select(i => i)
                .Where(p => p.Group == entity.CreateGroup.Id)
                .ToList();
                string consu = "";
                for (int i = 0; i < list.Count(); i++)
                {
                    int count = 0;
                    for (int j = 0; j < list.Count(); j++)
                    {
                        if (list[i].Amount <= list[i].Amount && list[i].Amount >= entity.TotalValue)
                        {
                            count++;
                            consu = list[i].User.ToString();
                        }
                    }
                    if (count == list.Count())
                    {
                        entity.UserAssigned = await _userHelper.GetUserByIdAsync(consu);
                        break;
                    }
                }
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha enviado la cotización de la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Autorizar -- MAIL OK
            if (model.Operation == 3)
            {
                foreach (ShoppingItemsEntity art in model.Items)
                {
                    if (art.State.Nombre == "Nuevo")
                    {
                        art.State = _context.TP_Shopping_Item_State.SingleOrDefault(o => o.Nombre == "Autorizado");
                        _context.Update(art);
                    }
                }
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Autorizado");
                entity.AssignedGroup = await _context.TP_Groups.FirstOrDefaultAsync(g => g.Name == "Compras");
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Core.ShoppingLogical ShopLog = new Core.ShoppingLogical();
                ShopLog.UpdShopingToNull(new Data.Connection.ShopingToNull { Id = entity.Id, Op = 1 }, _configuration.GetConnectionString("RefosusDesarrollo"));
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha autorizado la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Rechazar --MAIL OK
            if (model.Operation == 4)
            {
                foreach (ShoppingItemsEntity art in model.Items)
                {
                    art.State = _context.TP_Shopping_Item_State.SingleOrDefault(o => o.Nombre == "Rechazado");
                    _context.Update(art);

                }
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Rechazado");
                entity.AssignedGroup = entity.CreateGroup;
                entity.UserAssigned = entity.UserCreate;
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha rechazado la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Entregar articulo -- MAIL OK
            if (model.Operation == 5)
            {
                int cont = 0;
                foreach (ShoppingItemsEntity item in model.Items)
                {
                    if (item.Id == model.EditItem)
                    {
                        item.State = _context.TP_Shopping_Item_State.SingleOrDefault(o => o.Nombre == "Entregado");
                        _context.Update(item);
                    }
                    if (item.State.Nombre != "Entregado" && item.State.Nombre != "Rechazado")
                    {
                        cont++;
                    }
                }
                if (cont > 0)
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "En Entrega");
                }
                else
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Entregado");
                }
                entity.AssignedGroup = entity.CreateGroup;
                entity.UserAssigned = entity.UserCreate;
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha entregado el articulo de la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Entrega Completa -- MAIL OK
            if (model.Operation == 6)
            {
                foreach (ShoppingItemsEntity art in model.Items)
                {
                    if (art.State.Nombre == "Autorizado")
                    {
                        art.State = _context.TP_Shopping_Item_State.SingleOrDefault(o => o.Nombre == "Entregado");
                        _context.Update(art);
                    }
                }
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Entregado");
                entity.AssignedGroup = entity.CreateGroup;
                entity.UserAssigned = entity.UserCreate;
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha entregado la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Cancelar -- MAIL OK
            if (model.Operation == 7)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Cancelado");
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha cancelado la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Devolver compra -- MAIL OK
            if (model.Operation == 8)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Devuelto");
                entity.UserAssigned = entity.UserCreate;
                entity.AssignedGroup = entity.CreateGroup;
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha devuelto la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Enviar a Compras -- MAIL OK
            if (model.Operation == 9)
            {
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "En Cotización");
                entity.AssignedGroup = await _context.TP_Groups.FirstOrDefaultAsync(g => g.Name == "Compras");
                entity.UserAssigned = null;
                model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                .Where(i => i.UserCreate == model.UserCreate)
                .Where(i => i.CreateDate == model.CreateDate)
                .Where(i => i.UpdateDate == model.UpdateDate)
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
                        Reference = item.Reference,
                        Mark = item.Mark,
                        InternalOrder = item.InternalOrder,
                        NumInternalOrder = item.NumInternalOrder,
                        Shoping = entity
                    };
                    if (envio == 1)
                    {
                        objItem.UserAssigned = await _userHelper.GetUserByIdAsync(_context.ShoppingCategories.Include(o => o.Responsable).SingleAsync(o => o.Id == objItem.Category.Id).Result.Responsable.Id);
                    }
                    else
                        if (envio == 2)
                    {
                        objItem.UserAssigned = await _userHelper.GetUserByIdAsync(_context.ShoppingCategories.Include(o => o.Responsable).SingleAsync(o => o.Id == objItem.SubCategory.Id).Result.Responsable.Id);
                    }
                    else
                    {
                        objItem.UserAssigned = null;
                    }
                    _context.Add(objItem);
                    _context.Remove(item);
                }
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Core.ShoppingLogical ShopLog = new Core.ShoppingLogical();
                ShopLog.UpdShopingToNull(new Data.Connection.ShopingToNull { Id = entity.Id, Op = 1 }, _configuration.GetConnectionString("RefosusDesarrollo"));
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha enviado la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Agregar articulo
            if (model.Operation == 10)
            {
                //Add Item to temporal Table
                ShoppingTempItems itm = new ShoppingTempItems
                {
                    CodSAP = model.ItemCodSap,
                    Description = model.ItemDescription,
                    CreateDate = model.CreateDate,
                    UpdateDate = model.UpdateDate,
                    UserCreate = model.UserCreate,
                    Category = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.ItemIdCategory),
                    SubCategory = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.ItemIdSubCategory),
                    Unit = await _context.ShoppingUnits.FirstOrDefaultAsync(u => u.Id == model.ItemIdUnit),
                    Measure = await _context.ShoppingMeasures.FirstOrDefaultAsync(u => u.Id == model.ItemIdMeasure),
                    Quantity = model.ItemQuantity,
                    Reference = model.ItemReference,
                    Mark = model.ItemMark,
                    InternalOrder = model.ItemInternalOrder,
                    NumInternalOrder = model.ItemNumInternalOrder,
                    Observation = model.ItemObservation
                };
                _context.Add(itm);
                await _context.SaveChangesAsync();
                model = await returnModelAsync(model);
                return View(model);
            }
            //Actualizar Articulo
            if (model.Operation == 11)
            {
                if (model.EditTempItem > 0)
                {
                    ShoppingTempItems itm = new ShoppingTempItems
                    {
                        Id = model.EditTempItem,
                        CodSAP = model.ItemCodSap,
                        Description = model.ItemDescription,
                        CreateDate = model.CreateDate,
                        UpdateDate = model.UpdateDate,
                        UserCreate = model.UserCreate,
                        Category = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.ItemIdCategory),
                        SubCategory = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.ItemIdSubCategory),
                        Unit = await _context.ShoppingUnits.FirstOrDefaultAsync(u => u.Id == model.ItemIdUnit),
                        Measure = await _context.ShoppingMeasures.FirstOrDefaultAsync(u => u.Id == model.ItemIdMeasure),
                        Quantity = model.ItemQuantity,
                        Reference = model.ItemReference,
                        Mark = model.ItemMark,
                        InternalOrder = model.ItemInternalOrder,
                        NumInternalOrder = model.ItemNumInternalOrder,
                        Observation = model.ItemObservation
                    };
                    _context.Update(itm);
                }
                if (model.EditItem > 0)
                {
                    ShoppingItemsEntity itm = new ShoppingItemsEntity
                    {
                        Id = model.EditItem,
                        Shoping = entity,
                        CodSAP = model.ItemCodSap,
                        Category = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.ItemIdCategory),
                        SubCategory = await _context.ShoppingCategories.FirstOrDefaultAsync(c => c.Id == model.ItemIdSubCategory),
                        Unit = await _context.ShoppingUnits.FirstOrDefaultAsync(u => u.Id == model.ItemIdUnit),
                        Measure = await _context.ShoppingMeasures.FirstOrDefaultAsync(u => u.Id == model.ItemIdMeasure),
                        Quantity = model.ItemQuantity,
                        Description = model.ItemDescription,
                        Reference = model.ItemReference,
                        Mark = model.ItemMark,
                        InternalOrder = model.ItemInternalOrder,
                        NumInternalOrder = model.ItemNumInternalOrder,
                        Observation = model.ItemObservation,
                        QuantityDelivered = 0,
                        UserAssigned = null,
                        ValorTotal = model.ItemFullCost,
                        ValorUnidad = model.ItemUnitCost,
                        State = await _context.TP_Shopping_Item_State.SingleOrDefaultAsync(o => o.Nombre == "Nuevo")
                    };
                    if (envio == 1)
                    {
                        itm.UserAssigned = await _userHelper.GetUserByIdAsync(_context.ShoppingCategories.Include(o => o.Responsable).SingleAsync(o => o.Id == itm.Category.Id).Result.Responsable.Id);
                    }
                    else
                    if (envio == 2)
                    {
                        itm.UserAssigned = await _userHelper.GetUserByIdAsync(_context.ShoppingCategories.Include(o => o.Responsable).SingleAsync(o => o.Id == itm.SubCategory.Id).Result.Responsable.Id);
                    }
                    else
                    {
                        itm.UserAssigned = null;
                    }
                    _context.Update(itm);
                }


                await _context.SaveChangesAsync();
                model = await returnModelAsync(model);
                return View(model);

            }
            //Eliminar Articulo
            if (model.Operation == 12)
            {
                if (model.DeleteItemTemp > 0)
                {
                    ShoppingTempItems Item = await _context.ShoppingTempItems.FirstOrDefaultAsync(m => m.Id == model.DeleteItemTemp);
                    _context.Remove(Item);
                }
                if (model.DeleteItem > 0)
                {
                    ShoppingItemsEntity Item = await _context.ShoppingItems.FirstOrDefaultAsync(m => m.Id == model.DeleteItem);
                    _context.Remove(Item);
                }


                await _context.SaveChangesAsync();
                model = await returnModelAsync(model);
                return View(model);

            }
            //Autorizar articulo -- MAIL OK
            if (model.Operation == 13)
            {
                int cont = 0;
                foreach (ShoppingItemsEntity item in model.Items)
                {
                    if (item.Id == model.EditItem)
                    {
                        item.State = _context.TP_Shopping_Item_State.SingleOrDefault(o => o.Nombre == "Autorizado");
                        _context.Update(item);
                    }
                    if (item.State.Nombre != "Autorizado" && item.State.Nombre != "Rechazado")
                    {
                        cont++;
                    }
                }
                if (cont > 0)
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Por Autorización");
                }
                else
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Autorizado");
                }
                entity.AssignedGroup = entity.CreateGroup;
                entity.UserAssigned = entity.UserCreate;
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha autorizado un articulo de la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Agregar Proveedor
            if (model.Operation == 14)
            {
                TP_Shopping_ItemProvedorEntity obj = new TP_Shopping_ItemProvedorEntity()
                {
                    Name = model.ProoNombre,
                    Telefono = model.ProoTelefono,
                    Cantidad = model.ProoCantidad,
                    PrecioUnidad = model.ProoValorUnidad,
                    PrecioTotal = model.ProoCantidad * model.ProoValorUnidad,
                    Item = await _context.ShoppingItems.SingleOrDefaultAsync(o => o.Id == model.EditItem)
                };
                #region Load Files
                string Files = "";
                if (model.File != null)
                {
                    string Nombre;
                    foreach (Microsoft.AspNetCore.Http.IFormFile item in model.File)
                    {
                        obj.FileName = item.FileName;
                        obj.FilePath = await _fileHelper.UploadFileShoppingAsync(item, "Proveedor");
                    }
                }
                #endregion 
                _context.Add(obj);
                await _context.SaveChangesAsync();
                model = await returnModelAsync(model);
                return View(model);
            }
            //Aceptar compra -- MAIL OK
            if (model.Operation == 15)
            {
                foreach (ShoppingItemsEntity art in model.Items)
                {
                    if (art.State.Nombre == "Entregado")
                    {
                        art.State = _context.TP_Shopping_Item_State.SingleOrDefault(o => o.Nombre == "Finalizado");
                        _context.Update(art);
                    }
                }
                entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Finalizado");
                entity.AssignedGroup = entity.CreateGroup;
                entity.UserAssigned = entity.UserCreate;
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha aceptado la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Aceptar Articulo -- MAIL OK
            if (model.Operation == 16)
            {
                int cont = 0;
                foreach (ShoppingItemsEntity item in model.Items)
                {
                    if (item.Id == model.EditItem)
                    {
                        item.State = _context.TP_Shopping_Item_State.SingleOrDefault(o => o.Nombre == "Entregado");
                        _context.Update(item);
                    }
                    if (item.State.Nombre != "Entregado" && item.State.Nombre != "Rechazado")
                    {
                        cont++;
                    }
                }
                if (cont > 0)
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "En entrega");
                }
                else
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Entregado");
                }
                entity.AssignedGroup = entity.CreateGroup;
                entity.UserAssigned = entity.UserCreate;
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha aceptado un articulo de la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Rechazar Articulo -- MAIL OK
            if (model.Operation == 17)
            {
                int cont = 0;
                int cont2 = 0;
                foreach (ShoppingItemsEntity item in model.Items)
                {
                    if (item.Id == model.EditItem)
                    {
                        item.State = _context.TP_Shopping_Item_State.SingleOrDefault(o => o.Nombre == "Rechazado");
                        _context.Update(item);
                    }
                    if (item.State.Nombre != "Autorizado" && item.State.Nombre != "Rechazado")
                    {
                        cont++;
                    }
                    if (item.State.Nombre == "Rechazado")
                    {
                        cont2++;
                    }
                }
                if (cont > 0)
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Por Autorización");
                }
                else
                {
                    if (cont2 == model.Items.Count())
                    {
                        entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Rechazado");
                    }
                    else
                    {
                        entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Autorizado");
                    }
                }
                entity.AssignedGroup = entity.CreateGroup;
                entity.UserAssigned = entity.UserCreate;
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha rechazado un articulo de la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            //Recibir Articulo -- MAIL OK
            if (model.Operation == 18)
            {
                int cont = 0;
                foreach (ShoppingItemsEntity item in model.Items)
                {
                    if (item.Id == model.EditItem)
                    {
                        item.State = _context.TP_Shopping_Item_State.SingleOrDefault(o => o.Nombre == "Finalizado");
                        _context.Update(item);
                    }
                    if (item.State.Nombre != "Finalizado" && item.State.Nombre != "Rechazado")
                    {
                        cont++;
                    }
                }
                if (cont > 0)
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Entregado");
                }
                else
                {
                    entity.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Name == "Finalizado");
                }
                entity.AssignedGroup = entity.CreateGroup;
                entity.UserAssigned = entity.UserCreate;
                entity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(entity);
                await _context.SaveChangesAsync();
                Id = entity.Id + "-" + model.Pantalla;
                #region EMAIL
                string Asunto = "Compra  No. " + entity.Id;
                string Body = "Se ha recibido un articulo de la compra No <strong>" + entity.Id + "</strong>,  " +
                "por el usuario <strong>" + user.FullName + "</strong> y asignado a <strong>";
                if (entity.UserAssigned != null)
                {
                    Body += "el usuario " + entity.UserAssigned.FullName + ".</strong> <br/> ";
                }
                else
                {
                    Body += "el grupo de " + entity.AssignedGroup.Name + ".</strong> <br/> ";
                }
                List<string> to = new List<string>
                {
                    entity.UserCreate.Email,
                    user.Email
                };
                if (entity.UserAssigned != null)
                {
                    to.Add(entity.UserAssigned.Email);
                }
                else
                {
                    to.Add(entity.AssignedGroup.Email);
                }
                _mailHelper.sendMailHTML("Compras", Body, Asunto, to);
                #endregion
                return RedirectToAction(nameof(DetailsShopping), new { Id });
            }
            model = await returnModelAsync(model);
            return View(model);
        }
        #endregion
        #region ServicesCombo
        [Authorize]
        public async Task<string> UserProjectBoss(int UBP)
        {
            string cod = _context.Projects.FirstOrDefaultAsync(p => p.Id == UBP).Result.IdUserProjectBoss;
            return cod;
        }
        [Authorize]
        public async Task<IEnumerable> CombosSubCategory(int val)
        {
            IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> list = _combosHelper.GetComboShoppingCategory(val);
            return list;
        }
        [Authorize]
        public async Task<IEnumerable> CombosMeasure(int val)
        {
            IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> list = _combosHelper.GetComboShoppingMeasure(val);
            return list;
        }
        #endregion
        #region Helper
        [Authorize]
        public async Task<ShoppingViewModel> returnModelAsync(ShoppingViewModel model)
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            model.Companies = _combosHelper.GetComboCompany();
            model.ShoppingUnits = _combosHelper.GetComboShoppingUnit();
            model.ShoppingMeasures = _combosHelper.GetComboShoppingMeasure(model.ItemIdUnit);
            model.Categories = _combosHelper.GetComboShoppingCategory();
            model.SubCategories = _combosHelper.GetComboShoppingCategory(model.ItemIdCategory);
            model.Users = _combosHelper.GetComboUser();
            model.ShoppingStates = _combosHelper.GetComboShoppingState();
            model.Projects = _combosHelper.GetComboProject();
            model.Groups = _combosHelper.GetGroups();
            model.ItemsTemp = _context.ShoppingTempItems.Select(i => i)
                .Where(i => i.UserCreate == user)
                .Where(i => i.CreateDate == model.CreateDate)
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                .Include(t => t.Unit)
                .Include(t => t.Measure);
            return (model);
        }
        [Authorize]
        public async Task<ShoppingTempItems> ItemTemp(int val)
        {
            ShoppingTempItems t = await _context.ShoppingTempItems
                .Include(o => o.Category)
                .ThenInclude(o => o.SubCategory)
                .Include(o => o.SubCategory)
                .Include(o => o.Unit)
                .Include(o => o.Measure)
                .ThenInclude(o => o.Unit)
                .FirstOrDefaultAsync(o => o.Id == val);
            return t;
        }
        [Authorize]
        public async Task<ShoppingItemsEntity> Item(int val)
        {
            ShoppingItemsEntity t = await _context.ShoppingItems
                .Include(o => o.Category)
                .ThenInclude(o => o.SubCategory)
                .Include(o => o.SubCategory)
                .Include(o => o.Unit)
                .Include(o => o.Measure)
                .ThenInclude(o => o.Unit)
                .Include(o => o.Proveedores)
                .FirstOrDefaultAsync(o => o.Id == val);
            return t;
        }
        [Authorize]
        public async Task<TP_Shopping_ItemSAPEntity> ItemSAP(string val)
        {
            TP_Shopping_ItemSAPEntity t = await _context.TP_Shopping_ItemSAPEntity
                .Include(o => o.Category)
                .ThenInclude(o => o.SubCategory)
                .Include(o => o.SubCategory)
                .Include(o => o.Unit)
                .Include(o => o.Measure)
                .ThenInclude(o => o.Unit)
                .FirstOrDefaultAsync(o => o.CodSAP == val);
            if (t == null)
            {
                return new TP_Shopping_ItemSAPEntity()
                {
                    Id = 0,
                    CodSAP = "",
                    Description = "",
                    Category = _context.ShoppingCategories.FirstOrDefault(),
                    SubCategory = _context.ShoppingCategories.Include(o => o.SubCategory).FirstOrDefault(o => o.SubCategory == (_context.ShoppingCategories.FirstOrDefault())),
                    Unit = _context.ShoppingUnits.FirstOrDefault(),
                    Measure = _context.ShoppingMeasures.Include(o => o.Unit).FirstOrDefault(o => o.Unit == _context.ShoppingUnits.FirstOrDefault()),
                    InternalOrder = "",
                    Mark = "",
                    NumInternalOrder = "",
                    Reference = ""
                };
            }

            return t;
        }
        [Authorize]
        public async Task<List<TP_Shopping_ItemProvedorEntity>> ItemProveedor(int val)
        {
            ShoppingItemsEntity t = await _context.ShoppingItems
               .Include(o => o.Category)
               .ThenInclude(o => o.SubCategory)
               .Include(o => o.SubCategory)
               .Include(o => o.Unit)
               .Include(o => o.Measure)
               .ThenInclude(o => o.Unit)
               .Include(o => o.Proveedores)
               .FirstOrDefaultAsync(o => o.Id == val);
            return t.Proveedores;
        }
        #endregion

    }
}
