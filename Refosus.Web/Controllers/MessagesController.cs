using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{

    public class MessagesController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IUserHelper _userHelper;
        private readonly IFileHelper _fileHelper;
        private readonly IConverterHelper _converterHelper;

        public MessagesController(DataContext context,
            IConverterHelper converterHelper,
            ICombosHelper combosHelper,
            IUserHelper userHelper,
            IFileHelper fileHelper
            )
        {
            _context = context;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
            _fileHelper = fileHelper;
            _converterHelper = converterHelper;
        }
        #region Index

        [Authorize(Roles = "Administrator,MessageAdministrator")]
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context
                .Messages
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        [Authorize(Roles = "Administrator,MessageMeMessage")]
        public async Task<IActionResult> IndexMeAsync()
        {
            UserEntity Userme = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            return View(await _context
                .Messages
                .Where(t => (t.User.Id == Userme.Id && t.State.Name != "Tramitado") || ((t.UserSender.Id == Userme.Id && t.Type.Name == "Paquete" && t.State.Name != "Tramitado")))
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        [Authorize(Roles = "Administrator,MessageMeHistory")]
        public async Task<IActionResult> IndexMeHistory()
        {
            UserEntity Userme = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            return View(await _context
                .Messages
                .Where(t => (t.User.Id == Userme.Id && t.State.Name == "Tramitado") || ((t.UserSender.Id == Userme.Id && t.Type.Name == "Paquete" && t.State.Name == "Tramitado")))
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        [Authorize(Roles = "Administrator,MessageBillPending")]
        public async Task<IActionResult> IndexBillPendingAsync()
        {
            return View(await _context
                .Messages
                .Where(t => t.Type.Name == "Factura" && t.State.Name != "Tramitado")
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        [Authorize(Roles = "Administrator,MessageBillHistory")]
        public async Task<IActionResult> IndexBillHistoryAsync()
        {
            return View(await _context
                .Messages
                .Where(t => t.Type.Name == "Factura" && t.State.Name == "Tramitado")
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        #endregion

        #region Create
        [Authorize(Roles = "Administrator,MessageCreator")]
        public IActionResult CreateMessageAsync()
        {
            MessageViewModel model = new MessageViewModel
            {
                MessageType = _combosHelper.GetComboMessageType(),
                MessageState = _combosHelper.GetComboMessageState(),
                MessageBillState = _combosHelper.GetComboMessageBillState(),
                Users = _combosHelper.GetComboActiveUser()

            };
            return View(model);
        }

        [Authorize(Roles = "Administrator,MessageCreator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMessageAsync(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Transaction.Observation == null)
                {
                    ModelState.AddModelError(string.Empty, "El campo de observaciones no puede estar vacío");
                }
                else
                {
                    MessageEntity messageEntity = new MessageEntity();
                    messageEntity = await _converterHelper.ToMessageEntityAsync(model, true);
                    messageEntity.CreateDate = System.DateTime.Now.ToUniversalTime();
                    messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();

                    if (messageEntity.User == null)
                    {
                        messageEntity.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    }

                    if (messageEntity.Type.Name == "Paquete")
                    {
                        messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "En Transito");
                    }
                    else
                    {
                        messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "Ingresado");
                    }
                    if (messageEntity.Type.Name == "Factura")
                    {
                        messageEntity.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(o => o.Name == "Nuevo");
                    }
                    else
                    {
                        messageEntity.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(o => o.Name == "Otro");
                    }
                    messageEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    messageEntity.UserSender = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    _context.Add(messageEntity);

                    string Files = "";
                    if (model.File != null)
                    {
                        string ext;
                        string Nombre;

                        foreach (Microsoft.AspNetCore.Http.IFormFile item in model.File)
                        {
                            Nombre = item.FileName;
                            ext = Path.GetExtension(Nombre);
                            MessageFileEntity fileEntity = new MessageFileEntity
                            {
                                message = messageEntity,
                                Name = Nombre,
                                FilePath = await _fileHelper.UploadFileAsync(item, messageEntity.Type.Name),
                                Ext = ext
                            };
                            _context.Add(fileEntity);
                            Files += "\nEl usuario " + messageEntity.UserCreate.FullName
                                + " Agrega el archivo " + Nombre;
                        }

                    }
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
                    messagetransactionEntity = await _converterHelper.ToMessageTransactionEntityAsync(model);
                    messagetransactionEntity.StateCreate = messageEntity.State;
                    messagetransactionEntity.StateUpdate = messageEntity.State;
                    messagetransactionEntity.UpdateDate = messageEntity.UpdateDate;
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    messagetransactionEntity.UserUpdate = messageEntity.User;
                    messagetransactionEntity.Message = messageEntity;
                    string Description = "";
                    Description += "Se crea el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.CreateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                        ;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsMessage), new { id = messageEntity.Id });
                }
            }
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            model.Users = _combosHelper.GetComboActiveUser();
            return View(model);
        }

        [Authorize(Roles = "Administrator,MessageCreator")]
        public IActionResult CreateMeMessageAsync()
        {
            MessageViewModel model = new MessageViewModel
            {
                MessageType = _combosHelper.GetComboMessageType(),
                MessageState = _combosHelper.GetComboMessageState(),
                MessageBillState = _combosHelper.GetComboMessageBillState(),
                Users = _combosHelper.GetComboActiveUser()
            };
            return View(model);
        }

        [Authorize(Roles = "Administrator,MessageCreator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMeMessageAsync(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Transaction.Observation == null)
                {
                    ModelState.AddModelError(string.Empty, "El campo de observaciones no puede estar vacío");
                }
                else
                {
                    MessageEntity messageEntity = new MessageEntity();
                    messageEntity = await _converterHelper.ToMessageEntityAsync(model, true);
                    messageEntity.CreateDate = System.DateTime.Now.ToUniversalTime();
                    messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();

                    if (messageEntity.User == null)
                    {
                        messageEntity.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    }

                    if (messageEntity.Type.Name == "Paquete")
                    {
                        messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "En Transito");
                    }
                    else
                    {
                        messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "Ingresado");
                    }
                    if (messageEntity.Type.Name == "Factura")
                    {
                        messageEntity.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(o => o.Name == "Nuevo");
                    }
                    else
                    {
                        messageEntity.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(o => o.Name == "Otro");
                    }
                    messageEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    messageEntity.UserSender = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    _context.Add(messageEntity);

                    string Files = "";
                    if (model.File != null)
                    {
                        string ext;
                        string Nombre;

                        foreach (Microsoft.AspNetCore.Http.IFormFile item in model.File)
                        {
                            Nombre = item.FileName;
                            ext = Path.GetExtension(Nombre);
                            MessageFileEntity fileEntity = new MessageFileEntity
                            {
                                message = messageEntity,
                                Name = Nombre,
                                FilePath = await _fileHelper.UploadFileAsync(item, messageEntity.Type.Name),
                                Ext = ext
                            };
                            _context.Add(fileEntity);
                            Files += "\nEl usuario " + messageEntity.UserCreate.FullName
                                + " Agrega el archivo " + Nombre;
                        }

                    }
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
                    messagetransactionEntity = await _converterHelper.ToMessageTransactionEntityAsync(model);
                    messagetransactionEntity.StateCreate = messageEntity.State;
                    messagetransactionEntity.StateUpdate = messageEntity.State;
                    messagetransactionEntity.UpdateDate = messageEntity.UpdateDate;
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    messagetransactionEntity.UserUpdate = messageEntity.User;
                    messagetransactionEntity.Message = messageEntity;
                    string Description = "";
                    Description += "Se crea el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.CreateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                        ;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
                }
            }
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            model.Users = _combosHelper.GetComboActiveUser();
            return View(model);
        }
        #endregion

        #region Detaills
        [Authorize(Roles = "Administrator,MessageAdministrator")]
        public async Task<IActionResult> DetailsMessage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessageEntity messageEntity = await _context.Messages
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.StateBill)
                .Include(t => t.User)
                .Include(t => t.MessageFiles)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.UserCreate)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.StateCreate)
                .Include(t => t.Ceco)
                .Include(t=>t.Checks)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            return View(messageEntity);
        }
        [Authorize(Roles = "Administrator,MessageMeMessage")]
        public async Task<IActionResult> DetailsMeMessage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessageEntity messageEntity = await _context.Messages
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.StateBill)
                .Include(t => t.User)
                .Include(t => t.MessageFiles)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.UserCreate)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.StateCreate)
                .Include(t => t.Ceco)
                .Include(t => t.Checks)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            UserEntity Userme = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (!messageEntity.User.Email.Equals(Userme.Email) && !messageEntity.UserSender.Email.Equals(Userme.Email))
            {
                return View("../Account/NotAuthorized");
            }
            return View(messageEntity);
        }
        [Authorize(Roles = "Administrator,MessageMeHistory")]
        public async Task<IActionResult> DetailsMeHistoryMessageAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessageEntity messageEntity = await _context.Messages
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.StateBill)
                .Include(t => t.User)
                .Include(t => t.MessageFiles)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.UserCreate)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.StateCreate)
                .Include(t => t.Ceco)
                .Include(t => t.Checks)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            UserEntity Userme = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (!messageEntity.User.Email.Equals(Userme.Email) && !messageEntity.UserSender.Email.Equals(Userme.Email))
            {
                return View("../Account/NotAuthorized");
            }
            return View(messageEntity);
        }
        [Authorize(Roles = "Administrator,MessageBillPending")]
        public async Task<IActionResult> DetailsBillPendingMessageAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessageEntity messageEntity = await _context.Messages
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.StateBill)
                .Include(t => t.User)
                .Include(t => t.MessageFiles)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.UserCreate)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.StateCreate)
                .Include(t => t.Ceco)
                .Include(t => t.Checks)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            return View(messageEntity);
        }
        [Authorize(Roles = "Administrator,MessageBillHistory")]
        public async Task<IActionResult> DetailsBillHistoryMessageAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessageEntity messageEntity = await _context.Messages
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.StateBill)
                .Include(t => t.User)
                .Include(t => t.MessageFiles)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.UserCreate)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.StateCreate)
                .Include(t => t.Ceco)
                .Include(t => t.Checks)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            return View(messageEntity);
        }
        #endregion

        #region Transaction
        [Authorize(Roles = "Administrator,MessageAdministrator")]
        public async Task<IActionResult> DetailsTransaction(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessagetransactionEntity messagetransaction = await _context.MessagesTransaction
                .Include(t => t.Message)
                .Include(t => t.UserCreate)
                .Include(t => t.UserUpdate)
                .Include(t => t.StateCreate)
                .Include(t => t.StateUpdate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messagetransaction == null)
            {
                return NotFound();
            }
            return View(messagetransaction);
        }
        [Authorize(Roles = "Administrator,MessageMeMessage")]
        public async Task<IActionResult> DetailsMeTransaction(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessagetransactionEntity messagetransaction = await _context.MessagesTransaction
                .Include(t => t.Message)
                .Include(t => t.UserCreate)
                .Include(t => t.UserUpdate)
                .Include(t => t.StateCreate)
                .Include(t => t.StateUpdate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messagetransaction == null)
            {
                return NotFound();
            }
            return View(messagetransaction);
        }
        [Authorize(Roles = "Administrator,MessageMeHistory")]
        public async Task<IActionResult> DetailsMeHistoryTransaction(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessagetransactionEntity messagetransaction = await _context.MessagesTransaction
                .Include(t => t.Message)
                .Include(t => t.UserCreate)
                .Include(t => t.UserUpdate)
                .Include(t => t.StateCreate)
                .Include(t => t.StateUpdate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messagetransaction == null)
            {
                return NotFound();
            }
            return View(messagetransaction);
        }
        [Authorize(Roles = "Administrator,MessageBillPending")]
        public async Task<IActionResult> DetailsBillPendingTransaction(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessagetransactionEntity messagetransaction = await _context.MessagesTransaction
                .Include(t => t.Message)
                .Include(t => t.UserCreate)
                .Include(t => t.UserUpdate)
                .Include(t => t.StateCreate)
                .Include(t => t.StateUpdate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messagetransaction == null)
            {
                return NotFound();
            }
            return View(messagetransaction);
        }
        [Authorize(Roles = "Administrator,MessageBillHistory")]
        public async Task<IActionResult> DetailsBillHistoryTransaction(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessagetransactionEntity messagetransaction = await _context.MessagesTransaction
                .Include(t => t.Message)
                .Include(t => t.UserCreate)
                .Include(t => t.UserUpdate)
                .Include(t => t.StateCreate)
                .Include(t => t.StateUpdate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messagetransaction == null)
            {
                return NotFound();
            }
            return View(messagetransaction);
        }
        #endregion

        #region Edit
        [Authorize(Roles = "Administrator,MessageAdministrator")]
        public async Task<IActionResult> EditMessage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessageEntity messageEntity = await _context.Messages
                .Include(t => t.Type)
                .Include(t => t.User)
                .Include(t => t.State)
                .Include(t => t.StateBill)
                .Include(t => t.UserAut)
                .Include(t => t.UserPros)
                .Include(t => t.Ceco)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            MessageViewModel messageViewModel;
            if (messageEntity.UserAut == null && messageEntity.UserPros == null)
            {
                messageViewModel = _converterHelper.ToMessageViewModelNone(messageEntity);
            }
            else
            if (messageEntity.UserAut != null && messageEntity.UserPros == null)
            {
                messageViewModel = _converterHelper.ToMessageViewModelAut(messageEntity);
            }
            else
            {
                messageViewModel = _converterHelper.ToMessageViewModel(messageEntity);
            }

            return View(messageViewModel);
        }
        [Authorize(Roles = "Administrator,MessageAdministrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMessage(int id, MessageViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                MessageEntity messageEntity = await _converterHelper.ToMessageEntityAsync(model, false);
                messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                if (messageEntity.User != await _userHelper.GetUserByEmailAsync(User.Identity.Name))
                {
                    if (messageEntity.User == messageEntity.UserSender)
                    {
                        messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == 5);
                        messageEntity.UserSender = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    }
                    else
                    {
                        messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == 2);
                        messageEntity.UserSender = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    }
                }
                else
                {
                }
                _context.Update(messageEntity);

                string Files = "";
                if (model.File != null)
                {
                    string ext;
                    string Nombre;

                    foreach (Microsoft.AspNetCore.Http.IFormFile item in model.File)
                    {
                        Nombre = item.FileName;
                        ext = Path.GetExtension(Nombre);
                        MessageFileEntity fileEntity = new MessageFileEntity
                        {
                            message = messageEntity,
                            Name = Nombre,
                            FilePath = await _fileHelper.UploadFileAsync(item, messageEntity.Type.Name),
                            Ext = ext
                        };
                        _context.Add(fileEntity);
                        Files += "\nEl usuario " + messageEntity.UserCreate
                            + " Agrega el archivo " + Nombre;
                    }

                }

                MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
                messagetransactionEntity = await _converterHelper.ToMessageTransactionEntityAsync(model);

                messagetransactionEntity.StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Id == model.StateIdOld);
                messagetransactionEntity.StateUpdate = messageEntity.State;
                messagetransactionEntity.UpdateDate = messageEntity.UpdateDate;
                messagetransactionEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                messagetransactionEntity.UserUpdate = messageEntity.User;
                messagetransactionEntity.Message = messageEntity;
                string Description = "";
                Description += "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                    + " en la fecha " + messageEntity.UpdateDateLocal
                    + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                    + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                    + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                    + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                    ;
                Description += Files;
                messagetransactionEntity.Description = Description;
                _context.Add(messagetransactionEntity);


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsMessage), new { id = messageEntity.Id });
            }
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.Users = _combosHelper.GetComboActiveUser();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            return View(model);
        }
        [Authorize(Roles = "Administrator,MessageMeMessage")]
        public async Task<IActionResult> EditMeMessage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessageEntity messageEntity = await _context.Messages
                .Include(t => t.Type)
                .Include(t => t.User)
                .Include(t => t.State)
                .Include(t => t.StateBill)
                .Include(t => t.UserAut)
                .Include(t => t.UserPros)
                .Include(t => t.Ceco)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            UserEntity Userme = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (!messageEntity.User.Email.Equals(Userme.Email))
            {
                return View("../Account/NotAuthorized");
            }
            MessageViewModel messageViewModel;
            if (messageEntity.UserAut == null && messageEntity.UserPros == null)
            {
                messageViewModel = _converterHelper.ToMessageViewModelNone(messageEntity);
            }
            else
            if (messageEntity.UserAut != null && messageEntity.UserPros == null)
            {
                messageViewModel = _converterHelper.ToMessageViewModelAut(messageEntity);
            }
            else
            {
                messageViewModel = _converterHelper.ToMessageViewModel(messageEntity);
            }

            return View(messageViewModel);
        }
        [Authorize(Roles = "Administrator,MessageMeMessage")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMeMessage(int id, MessageViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (model.Transaction.Observation == null)
                {
                    ModelState.AddModelError(string.Empty, "El campo de observación no puede estar vacío");
                }
                else
                {

                    MessageEntity messageEntity = await _converterHelper.ToMessageEntityAsync(model, false);
                    messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                    if (messageEntity.User != await _userHelper.GetUserByEmailAsync(User.Identity.Name))
                    {
                        if (messageEntity.User == messageEntity.UserSender)
                        {
                            messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == 5);
                            messageEntity.UserSender = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                        }
                        else
                        {
                            messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == 2);
                            messageEntity.UserSender = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                        }
                    }
                    else
                    {
                    }
                    _context.Update(messageEntity);

                    string Files = "";
                    if (model.File != null)
                    {
                        string ext;
                        string Nombre;

                        foreach (Microsoft.AspNetCore.Http.IFormFile item in model.File)
                        {
                            Nombre = item.FileName;
                            ext = Path.GetExtension(Nombre);
                            MessageFileEntity fileEntity = new MessageFileEntity
                            {
                                message = messageEntity,
                                Name = Nombre,
                                FilePath = await _fileHelper.UploadFileAsync(item, messageEntity.Type.Name),
                                Ext = ext
                            };
                            _context.Add(fileEntity);
                            Files += "\nEl usuario " + messageEntity.UserCreate
                                + " Agrega el archivo " + Nombre;
                        }

                    }

                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
                    messagetransactionEntity = await _converterHelper.ToMessageTransactionEntityAsync(model);
                    messagetransactionEntity.StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Id == model.StateIdOld);
                    messagetransactionEntity.StateUpdate = messageEntity.State;
                    messagetransactionEntity.UpdateDate = messageEntity.UpdateDate;
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    messagetransactionEntity.UserUpdate = messageEntity.User;
                    messagetransactionEntity.Message = messageEntity;
                    string Description = "";
                    Description += "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                        ;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);


                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
                }
            }
            model.Type = await _context.MessagesTypes.FirstOrDefaultAsync(t => t.Id == model.TypeId);
            model.State = await _context.MessagesStates.FirstOrDefaultAsync(t => t.Id == model.StateId);
            model.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(t => t.Id == model.StateBillId);
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.Users = _combosHelper.GetComboActiveUser();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            return View(model);
        }

        #endregion

        #region Message
        /*
        public async Task<IActionResult> ReceiveMessageAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modelEntity == null)
            {
                return NotFound();
            }
            string Factura = "";
            modelEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            MessageStateEntity stateOld = modelEntity.State;
            if (modelEntity.Type.Id == 3)
            {
                modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == 6);
            }
            else
            {
                if (modelEntity.Type.Id == 1)
                {
                    modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == 4);
                }
                else
                {
                    if (modelEntity.Type.Id == 2)
                    {
                        modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == 7);
                        Factura += " se cambia el estado de la factura de " + modelEntity.StateBill.Name;
                        modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Id == 1);
                        Factura += " por el estado " + modelEntity.StateBill.Name;
                    }
                }
            }
            DateTime update = System.DateTime.Now.ToUniversalTime();
            modelEntity.UpdateDate = update;
            _context.Update(modelEntity);

            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                + Factura
                ;

            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);


            await _context.SaveChangesAsync();
            return RedirectToAction("EditMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "EditMessage", Id = modelEntity.Id }));
        }
        public async Task<IActionResult> FinishedMessageAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modelEntity == null)
            {
                return NotFound();
            }
            string Factura = "";
            MessageStateEntity stateOld = modelEntity.State;
            DateTime update = System.DateTime.Now.ToUniversalTime();
            if (modelEntity.Type.Id != 2)
            {
                modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == 4);
                modelEntity.UpdateDate = update;
            }
            else
            {
                UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                MessageBillStateEntity billstateold = modelEntity.StateBill;
                modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == 4);
                modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Id == 4);
                modelEntity.UserPros = user;
                modelEntity.DateProcess = update;
                modelEntity.UpdateDate = update;

                Factura += " se cambia el estado de la factura de " + billstateold.Name
                + " por el estado " + modelEntity.StateBill.Name
                + " se autorizo la factura por el usuario " + modelEntity.UserAut.FullName
                + " a las " + modelEntity.DateAutLocal
                + " se finaliza su proceso por el usuario " + modelEntity.UserPros.FullName
                + " a las " + modelEntity.DateProcessLocal
                ;
            }
            _context.Update(modelEntity);

            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                + Factura
                ;

            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);


            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new RouteValueDictionary(
                    new { controller = "Messages", action = "Index", Id = modelEntity.Id }));
        }
        public async Task<IActionResult> AuthorizeMessageBillAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
            {
                return NotFound();
            }
            string Factura = "";
            modelEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            MessageStateEntity stateOld = modelEntity.State;

            UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            MessageBillStateEntity billstateold = modelEntity.StateBill;
            DateTime update = System.DateTime.Now.ToUniversalTime();
            modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Id == 2);
            modelEntity.UserAut = user;
            modelEntity.DateAut = update;
            modelEntity.UpdateDate = update;

            _context.Update(modelEntity);


            Factura += " se cambia el estado de la factura de " + billstateold.Name
                + " por el estado " + modelEntity.StateBill.Name
                + " se autoriza la factura por el usuario " + modelEntity.UserAut.FullName
                + " a las " + modelEntity.DateAutLocal
                ;

            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                + Factura
                ;

            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);


            await _context.SaveChangesAsync();
            return RedirectToAction("EditMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "EditMessage", Id = modelEntity.Id }));
        }
        public async Task<IActionResult> RefuseMessageBillAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
            {
                return NotFound();
            }

            string Factura = "";
            MessageStateEntity stateOld = modelEntity.State;

            UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            MessageBillStateEntity billstateold = modelEntity.StateBill;
            DateTime update = System.DateTime.Now.ToUniversalTime();
            modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == 4);
            modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Id == 3);
            modelEntity.UserAut = user;
            modelEntity.DateAut = update;
            modelEntity.UserPros = user;
            modelEntity.DateProcess = update;
            modelEntity.UpdateDate = update;

            _context.Update(modelEntity);


            Factura += " se cambia el estado de la factura de " + billstateold.Name
                + " por el estado " + modelEntity.StateBill.Name
                + " se rechaza la factura por el usuario " + modelEntity.UserAut.FullName
                + " a las " + modelEntity.DateAutLocal
                + " se finaliza su proceso por el usuario " + modelEntity.UserPros.FullName
                + " a las " + modelEntity.DateProcessLocal
                ;

            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                + Factura
                ;

            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);

            await _context.SaveChangesAsync();
            return RedirectToAction("EditMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "EditMessage", Id = modelEntity.Id }));
        }

        */

        [Authorize]
        public async Task<IActionResult> ReceiveMessageAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modelEntity == null)
            {
                return View();
            }
            modelEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            MessageStateEntity stateOld = modelEntity.State;
            if (modelEntity.Type.Name == "Carta")
            {
                modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
            }
            else
            {
                if (modelEntity.Type.Name == "Paquete")
                {
                    modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                }
            }
            DateTime update = System.DateTime.Now.ToUniversalTime();
            modelEntity.UpdateDate = update;
            _context.Update(modelEntity);

            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                ;
            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);


            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "DetailsMessage", Id = modelEntity.Id }));
        }
        [Authorize(Roles = "Administrator,MessageBillProcesator")]
        public async Task<IActionResult> FinishedMessageAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
            {
                return NotFound();
            }
            string Factura = "";
            MessageStateEntity stateOld = modelEntity.State;
            DateTime update = System.DateTime.Now.ToUniversalTime();
            if (modelEntity.Type.Name != "Factura")
            {
                modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                modelEntity.UpdateDate = update;
            }
            else
            {
                UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                MessageBillStateEntity billstateold = modelEntity.StateBill;
                modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
                modelEntity.UserPros = user;
                modelEntity.DateProcess = update;
                modelEntity.UpdateDate = update;
                Factura += " se cambia el estado de la factura de " + billstateold.Name
                + " por el estado " + modelEntity.StateBill.Name
                + " se autorizo la factura por el usuario " + modelEntity.UserAut.FullName
                + " a las " + modelEntity.DateAutLocal
                + " se finaliza su proceso por el usuario " + modelEntity.UserPros.FullName
                + " a las " + modelEntity.DateProcessLocal
                ;
            }
            _context.Update(modelEntity);
            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                + Factura
                ;
            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "DetailsMessage", Id = modelEntity.Id }));
        }
        [Authorize(Roles = "Administrator,MessageBillAutorizador")]
        public async Task<IActionResult> AuthorizeMessageBillAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
            {
                return NotFound();
            }
            string Factura = "";
            modelEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            MessageStateEntity stateOld = modelEntity.State;
            UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            MessageBillStateEntity billstateold = modelEntity.StateBill;
            DateTime update = System.DateTime.Now.ToUniversalTime();
            modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Aprobado");
            modelEntity.UserAut = user;
            modelEntity.DateAut = update;
            modelEntity.UpdateDate = update;
            _context.Update(modelEntity);
            Factura += " se cambia el estado de la factura de " + billstateold.Name
                + " por el estado " + modelEntity.StateBill.Name
                + " se autoriza la factura por el usuario " + modelEntity.UserAut.FullName
                + " a las " + modelEntity.DateAutLocal
                ;
            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                + Factura
                ;
            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "DetailsMessage", Id = modelEntity.Id }));
        }
        [Authorize(Roles = "Administrator,MessageBillAutorizador")]
        public async Task<IActionResult> RefuseMessageBillAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
            {
                return NotFound();
            }

            string Factura = "";
            MessageStateEntity stateOld = modelEntity.State;

            UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            MessageBillStateEntity billstateold = modelEntity.StateBill;
            DateTime update = System.DateTime.Now.ToUniversalTime();
            modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
            modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
            modelEntity.UserAut = user;
            modelEntity.DateAut = update;
            modelEntity.UserPros = user;
            modelEntity.DateProcess = update;
            modelEntity.UpdateDate = update;

            _context.Update(modelEntity);


            Factura += " se cambia el estado de la factura de " + billstateold.Name
                + " por el estado " + modelEntity.StateBill.Name
                + " se rechaza la factura por el usuario " + modelEntity.UserAut.FullName
                + " a las " + modelEntity.DateAutLocal
                + " se finaliza su proceso por el usuario " + modelEntity.UserPros.FullName
                + " a las " + modelEntity.DateProcessLocal
                ;

            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                + Factura
                ;
            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "DetailsMessage", Id = modelEntity.Id }));
        }
        [Authorize(Roles = "Administrator,MessageBillChecker")]
        public async Task<IActionResult> CheckMessageBillAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
            {
                return NotFound();
            }
            UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            DateTime update = System.DateTime.Now.ToUniversalTime();

            MessageCheckEntity check = new MessageCheckEntity();
            check.message = modelEntity;
            check.User = user;
            check.DateAut = update;
            _context.Add(check);

            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = modelEntity.State,
                StateUpdate = modelEntity.State,
                UpdateDate = update,
                UserCreate = user,
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se da el visto bueno por el usuario " + user.FullName
                + " en la fecha " + check.DateAutLocal;
            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "DetailsMessage", Id = modelEntity.Id }));
        }
        #endregion

        #region MeMessage
        [Authorize]
        public async Task<IActionResult> ReceiveMeMessageAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modelEntity == null)
            {
                return View();
            }
            modelEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            MessageStateEntity stateOld = modelEntity.State;
            if (modelEntity.Type.Name == "Carta")
            {
                modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
            }
            else
            {
                if (modelEntity.Type.Name == "Paquete")
                {
                    modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                }
            }
            DateTime update = System.DateTime.Now.ToUniversalTime();
            modelEntity.UpdateDate = update;
            _context.Update(modelEntity);

            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                ;
            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);


            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsMeMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "DetailsMeMessage", Id = modelEntity.Id }));
        }
        [Authorize(Roles = "Administrator,MessageBillProcesator")]
        public async Task<IActionResult> FinishedMeMessageAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
            {
                return NotFound();
            }
            string Factura = "";
            MessageStateEntity stateOld = modelEntity.State;
            DateTime update = System.DateTime.Now.ToUniversalTime();
            if (modelEntity.Type.Name != "Factura")
            {
                modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                modelEntity.UpdateDate = update;
            }
            else
            {
                UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                MessageBillStateEntity billstateold = modelEntity.StateBill;
                modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
                modelEntity.UserPros = user;
                modelEntity.DateProcess = update;
                modelEntity.UpdateDate = update;
                Factura += " se cambia el estado de la factura de " + billstateold.Name
                + " por el estado " + modelEntity.StateBill.Name
                + " se autorizo la factura por el usuario " + modelEntity.UserAut.FullName
                + " a las " + modelEntity.DateAutLocal
                + " se finaliza su proceso por el usuario " + modelEntity.UserPros.FullName
                + " a las " + modelEntity.DateProcessLocal
                ;
            }
            _context.Update(modelEntity);
            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                + Factura
                ;
            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsMeMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "DetailsMeMessage", Id = modelEntity.Id }));
        }
        [Authorize(Roles = "Administrator,MessageBillAutorizador")]
        public async Task<IActionResult> AuthorizeMeMessageBillAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
            {
                return NotFound();
            }
            string Factura = "";
            modelEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            MessageStateEntity stateOld = modelEntity.State;
            UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            MessageBillStateEntity billstateold = modelEntity.StateBill;
            DateTime update = System.DateTime.Now.ToUniversalTime();
            modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Aprobado");
            modelEntity.UserAut = user;
            modelEntity.DateAut = update;
            modelEntity.UpdateDate = update;
            _context.Update(modelEntity);
            Factura += " se cambia el estado de la factura de " + billstateold.Name
                + " por el estado " + modelEntity.StateBill.Name
                + " se autoriza la factura por el usuario " + modelEntity.UserAut.FullName
                + " a las " + modelEntity.DateAutLocal
                ;
            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                + Factura
                ;
            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsMeMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "DetailsMeMessage", Id = modelEntity.Id }));
        }
        [Authorize(Roles = "Administrator,MessageBillAutorizador")]
        public async Task<IActionResult> RefuseMeMessageBillAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
            {
                return NotFound();
            }

            string Factura = "";
            MessageStateEntity stateOld = modelEntity.State;

            UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            MessageBillStateEntity billstateold = modelEntity.StateBill;
            DateTime update = System.DateTime.Now.ToUniversalTime();
            modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
            modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
            modelEntity.UserAut = user;
            modelEntity.DateAut = update;
            modelEntity.UserPros = user;
            modelEntity.DateProcess = update;
            modelEntity.UpdateDate = update;

            _context.Update(modelEntity);


            Factura += " se cambia el estado de la factura de " + billstateold.Name
                + " por el estado " + modelEntity.StateBill.Name
                + " se rechaza la factura por el usuario " + modelEntity.UserAut.FullName
                + " a las " + modelEntity.DateAutLocal
                + " se finaliza su proceso por el usuario " + modelEntity.UserPros.FullName
                + " a las " + modelEntity.DateProcessLocal
                ;

            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = stateOld,
                StateUpdate = modelEntity.State,
                UpdateDate = modelEntity.UpdateDate,
                UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name),
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se actualiza el mensaje de tipo " + modelEntity.Type.Name
                + " en la fecha " + messagetransactionEntity.UpdateDateLocal
                + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                + Factura
                ;
            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsMeMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "DetailsMeMessage", Id = modelEntity.Id }));
        }
        [Authorize(Roles = "Administrator,MessageBillChecker")]
        public async Task<IActionResult> CheckMeMessageBillAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
            {
                return NotFound();
            }
            UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            DateTime update = System.DateTime.Now.ToUniversalTime();

            MessageCheckEntity check = new MessageCheckEntity();
            check.message = modelEntity;
            check.User = user;
            check.DateAut = update;
            _context.Add(check);

            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
            {
                StateCreate = modelEntity.State,
                StateUpdate = modelEntity.State,
                UpdateDate = update,
                UserCreate = user,
                UserUpdate = modelEntity.User,
                Message = modelEntity,
                Observation = note
            };
            string Description = "";
            Description += "Se da el visto bueno por el usuario " + user.FullName
                + " en la fecha " + check.DateAutLocal                ;
            messagetransactionEntity.Description = Description;
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsMeMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "DetailsMeMessage", Id = modelEntity.Id }));
        }
        #endregion
    }
}