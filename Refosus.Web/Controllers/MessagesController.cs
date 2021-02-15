using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System;
using System.Collections.Generic;
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
        private readonly IMailHelper _mailHelper;
        private readonly IConverterHelper _converterHelper;
        public MessagesController(DataContext context,
            IConverterHelper converterHelper,
            ICombosHelper combosHelper,
            IUserHelper userHelper,
            IFileHelper fileHelper,
            IMailHelper mailHelper
            )
        {
            _context = context;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
            _fileHelper = fileHelper;
            _mailHelper = mailHelper;
            _converterHelper = converterHelper;
        }
        #region Index
        [Authorize(Roles = "Administrator,MessageAdministrator")]
        public async Task<IActionResult> Index()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            return View(await
                _context.Messages
                .Where(t => t.Company.Id == user.Company.Id)
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .Include(t => t.Company)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        [Authorize(Roles = "Administrator,MessageMeMessage")]
        public async Task<IActionResult> IndexMe()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            return View(await _context
                .Messages
                .Where(t => (t.User.Id == user.Id && t.State.Name != "Tramitado" && t.State.Name != "Recibido" && t.State.Name != "Anulado"))
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .Include(t => t.Company)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }

        [Authorize(Roles = "Administrator,MessageMeHistory")]
        public async Task<IActionResult> IndexMeHistory()
        {
            UserEntity Userme = await _userHelper.GetUserAsync(User.Identity.Name);
            return View(await _context
                .Messages
                .Where(t =>
                (((t.UserSender == Userme) || (t.User == Userme)) && ((t.State.Name == "Tramitado") || (t.State.Name == "Recibido") || (t.State.Name == "Anulado")))
                ||
                ((t.UserSender == Userme) && (t.User != Userme))
                ||
                ((t.UserCreate == Userme) && (t.UserSender != Userme) && (t.User != Userme))
                )
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.UserCreate)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .Include(t => t.Company)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        [Authorize(Roles = "Administrator,MessageBillPending")]
        public async Task<IActionResult> IndexBillPendingAsync()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            return View(await _context
                .Messages
                .Where(t => t.Type.Name == "Factura" && t.State.Name != "Tramitado" && t.State.Name != "Rechazado" && t.State.Name != "Anulado")
                .Where(t => t.Company.Id == user.Company.Id)
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .Include(t => t.Company)
                .Include(t => t.StateBill)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        [Authorize(Roles = "Administrator,MessageBillHistory")]
        public async Task<IActionResult> IndexBillHistoryAsync()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            return View(await _context
                .Messages
                .Where(t => t.Type.Name == "Factura" && (t.State.Name == "Tramitado" || t.State.Name == "Rechazado" || t.State.Name != "Anulado"))
                .Where(t => t.Company.Id == user.Company.Id)
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .Include(t => t.Company)
                .Include(t => t.StateBill)
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
                Users = _combosHelper.GetComboUser(),
                Companies = _combosHelper.GetComboCompany()
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
                #region Message
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                DateTime DateNow = System.DateTime.Now.ToUniversalTime();
                MessageEntity messageEntity = await _converterHelper.ToMessageEntityAsync(model, true);
                messageEntity.CreateDate = DateNow;
                messageEntity.UpdateDate = DateNow;
                messageEntity.UserCreate = user;
                messageEntity.UserSender = user;
                //si no se cambia el usuario se auto envia
                if (messageEntity.User == null)
                {
                    messageEntity.User = user;
                }
                //asigna el estado inicial del documento
                if (messageEntity.Type.Name == "Factura")
                {
                    messageEntity.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(o => o.Name == "Nuevo");
                    messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "Ingresado");
                }
                else
                {
                    if (messageEntity.Type.Name == "Paquete")
                    {
                        messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "En Transito");
                    }
                    else
                    {
                        messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "Ingresado");
                    }
                    messageEntity.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(o => o.Name == "Otro");
                }
                _context.Add(messageEntity);
                #endregion
                #region AddFile
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
                #endregion
                #region AddTransaction
                MessageBillStateEntity billstateold = _context.MessagesBillState.FirstOrDefault(s => s.Id == model.StateBillId);
                MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                {
                    Message = messageEntity,
                    UpdateDate = DateNow,
                    UserCreate = messageEntity.UserSender,
                    UserUpdate = messageEntity.User,
                    StateCreate = messageEntity.State,
                    StateUpdate = messageEntity.State,
                    StateBillCreate = billstateold,
                    StateBillUpdate = messageEntity.StateBill,
                    UserBillAutho = messageEntity.UserAut,
                    UserBillFinished = messageEntity.UserPros
                };
                ;
                messagetransactionEntity.Observation = model.Transaction.Observation;
                string Description = "Se crea el mensaje de tipo " + messageEntity.Type.Name
                    + " en la fecha " + messageEntity.UpdateDateLocal
                    + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                    + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                    + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                    + " y un estado final " + messagetransactionEntity.StateUpdate.Name;
                Description += Files;
                messagetransactionEntity.Description = Description;
                _context.Add(messagetransactionEntity);
                #endregion
                await _context.SaveChangesAsync();
                #region SendMail
                string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                string body =
                    "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia. <br/> Por favor no responda este mensaje. <br/>" +
                    " <br/> Hola. <br/> " +
                    "Se ha creado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, con referencia <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                    messageEntity.Id + ",</strong> con Remitente  <strong>" + messageEntity.Sender + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                    "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                    "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                    "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                    "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                    "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                    "una respuesta. <br/> <br/> " +
                    "Atentamente,<br/>" +
                    "Equipo de Soporte - Refocosta.<br/>";
                string[] to = new string[2];
                to[0] = messagetransactionEntity.UserCreate.Email;
                to[1] = messagetransactionEntity.UserUpdate.Email;
                _mailHelper.sendMail(to, subject, body);
                #endregion
                return RedirectToAction(nameof(DetailsMessage), new { messageEntity.Id });
            }
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            model.Users = _combosHelper.GetComboUser();
            model.Companies = _combosHelper.GetComboCompany();
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
                Users = _combosHelper.GetComboUser(),
                Companies = _combosHelper.GetComboCompany()
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
                #region Message
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                DateTime DateNow = System.DateTime.Now.ToUniversalTime();
                MessageEntity messageEntity = await _converterHelper.ToMessageEntityAsync(model, true);
                messageEntity.CreateDate = DateNow;
                messageEntity.UpdateDate = DateNow;
                messageEntity.UserCreate = user;
                messageEntity.UserSender = user;
                //si no se cambia el usuario se auto envia
                if (messageEntity.User == null)
                {
                    messageEntity.User = user;
                }
                //asigna el estado inicial del documento
                if (messageEntity.Type.Name == "Factura")
                {
                    messageEntity.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(o => o.Name == "Nuevo");
                    messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "Ingresado");
                }
                else
                {
                    if (messageEntity.Type.Name == "Paquete")
                    {
                        messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "En Transito");
                    }
                    else
                    {
                        messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "Ingresado");
                    }
                    messageEntity.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(o => o.Name == "Otro");
                }
                _context.Add(messageEntity);
                #endregion
                #region AddFile
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
                #endregion
                #region AddTransaction
                MessageBillStateEntity billstateold = _context.MessagesBillState.FirstOrDefault(s => s.Id == model.StateBillId);
                MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                {
                    Message = messageEntity,
                    UpdateDate = DateNow,
                    UserCreate = messageEntity.UserSender,
                    UserUpdate = messageEntity.User,
                    StateCreate = messageEntity.State,
                    StateUpdate = messageEntity.State,
                    StateBillCreate = billstateold,
                    StateBillUpdate = messageEntity.StateBill,
                    UserBillAutho = messageEntity.UserAut,
                    UserBillFinished = messageEntity.UserPros
                };
                ;
                messagetransactionEntity.Observation = model.Transaction.Observation;
                string Description = "Se crea el mensaje de tipo " + messageEntity.Type.Name
                    + " en la fecha " + messageEntity.UpdateDateLocal
                    + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                    + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                    + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                    + " y un estado final " + messagetransactionEntity.StateUpdate.Name;
                Description += Files;
                messagetransactionEntity.Description = Description;
                _context.Add(messagetransactionEntity);
                #endregion
                await _context.SaveChangesAsync();
                #region SendMail
                string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                string body =
                    "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia. <br/> Por favor no responda este mensaje. <br/>" +
                    " <br/> Hola. <br/> " +
                    "Se ha creado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, con referencia <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                    messageEntity.Id + ",</strong> con Remitente <strong>" + messageEntity.Sender + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                    "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                    "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                    "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                    "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                    "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                    "una respuesta. <br/> <br/> " +
                    "Atentamente,<br/>" +
                    "Equipo de Soporte - Refocosta.<br/>";
                string[] to = new string[2];
                to[0] = messagetransactionEntity.UserCreate.Email;
                to[1] = messagetransactionEntity.UserUpdate.Email;
                _mailHelper.sendMail(to, subject, body);
                #endregion
                return RedirectToAction(nameof(DetailsMeMessage), new { messageEntity.Id });
            }
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            model.Users = _combosHelper.GetComboUser();
            model.Companies = _combosHelper.GetComboCompany();
            return View(model);
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
                .Include(t => t.Checks)
                .Include(t => t.Company)
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
                .Include(t => t.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            if (!messageEntity.User.Email.Equals(User.Identity.Name) && !messageEntity.UserSender.Email.Equals(User.Identity.Name))
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
                .Include(t => t.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            if (!messageEntity.User.Email.Equals(User.Identity.Name) && !messageEntity.UserSender.Email.Equals(User.Identity.Name) && !messageEntity.UserCreate.Email.Equals(User.Identity.Name))
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
                .Include(t => t.Company)
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
                .Include(t => t.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            return View(messageEntity);
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
                .Include(t => t.UserCreate)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.State)
                .Include(t => t.StateBill)
                .Include(t => t.Ceco)
                .Include(t => t.Company)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            MessageViewModel messageViewModel = _converterHelper.ToMessageViewModel(messageEntity);
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
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                MessageEntity messageEntity = await _converterHelper.ToMessageEntityAsync(model, false);
                MessageBillStateEntity billstateold = _context.MessagesBillState.FirstOrDefault(s => s.Id == model.StateBillId);
                messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                messageEntity.UserSender = user;
                #region Load Files
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
                        Files += "\nEl usuario " + user.FullName
                            + " Agrega el archivo " + Nombre;
                    }
                }
                #endregion 
                //Operation 1 Edit
                if (model.Operation == 1)
                {
                    #region Update Info
                    if (model.StateIdOld == messageEntity.State.Id)
                    {
                        messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "En Proceso");
                    }
                    else
                    {
                        messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Id == model.StateId);
                    }
                    //message = null;
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        Observation = model.Transaction.Observation
                    };
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name;
                    if (messageEntity.Type.Name == "Factura")
                    {
                        messagetransactionEntity.StateBillCreate = billstateold;
                        messagetransactionEntity.StateBillUpdate = messageEntity.StateBill;
                        messagetransactionEntity.UserBillAutho = messageEntity.UserAut;
                        messagetransactionEntity.UserBillFinished = messageEntity.UserPros;
                        string Factura = "\nSe cambia el estado de la factura de " + messagetransactionEntity.StateBillCreate.Name
                        + " por el estado " + messagetransactionEntity.StateBillUpdate.Name;
                        Description += Factura;
                    }
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    if (messagetransactionEntity.UserCreate != messagetransactionEntity.UserUpdate)
                    {
                        string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                        string body =
                            "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                            " <br/> Hola. <br/> " +
                            "Se ha autorizado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                            messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                            "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                            "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                            "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                            "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                            "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                            "una respuesta. <br/> <br/> " +
                            "Atentamente,<br/>" +
                            "Equipo de Soporte - Refocosta.<br/>";
                        string[] to = new string[1];
                        to[0] = messagetransactionEntity.UserUpdate.Email;
                        _mailHelper.sendMail(to, subject, body);
                    }
                    #endregion
                    return RedirectToAction(nameof(DetailsMessage), new { id = messageEntity.Id });
                }
                //Operation 2 Authorize
                if (model.Operation == 2)
                {
                    #region Update Info
                    if (messageEntity.User == user)
                    {
                        if (messageEntity.Ceco.UserResponsible == null)
                        {
                            ViewBag.Mensaje = "Este centro de costos no cuenta con responsable. Seleccione el usuario de forma manual.";
                            model.Type = await _context.MessagesTypes.FirstOrDefaultAsync(t => t.Id == model.TypeId);
                            model.State = await _context.MessagesStates.FirstOrDefaultAsync(t => t.Id == model.StateId);
                            model.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(t => t.Id == model.StateBillId);
                            model.MessageType = _combosHelper.GetComboMessageType();
                            model.Companies = _combosHelper.GetComboCompany();
                            model.MessageState = _combosHelper.GetComboMessageState();
                            model.Users = _combosHelper.GetComboUser();
                            model.MessageBillState = _combosHelper.GetComboMessageBillState();
                            model.Cecos = _combosHelper.GetComboCeCo(model.CompanyId);
                            return View(model);
                        }
                        else
                        {
                            messageEntity.User = messageEntity.Ceco.UserResponsible;
                        }
                    }
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Aprobado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "En Proceso");
                    messageEntity.UserAut = user;
                    messageEntity.DateAut = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        StateBillCreate = billstateold,
                        StateBillUpdate = messageEntity.StateBill,
                        UserBillAutho = messageEntity.UserAut,
                        UserBillFinished = messageEntity.UserPros,
                        Observation = model.Transaction.Observation
                    };
                    string Factura = "\nSe cambia el estado de la factura de " + messagetransactionEntity.StateBillCreate.Name
                        + " por el estado " + messagetransactionEntity.StateBillUpdate.Name
                        + "\nse aprueba la factura por el usuario " + messagetransactionEntity.UserBillAutho.FullName
                        + " a las " + messageEntity.UpdateDateLocal;
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                        + Factura
                        ;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha autorizado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messagetransactionEntity.UserUpdate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                    return RedirectToAction(nameof(DetailsMessage), new { id = messageEntity.Id });
                }
                //Operation 3 Refuse
                if (model.Operation == 3)
                {
                    #region Update Info
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Rechazado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                    messageEntity.UserAut = user;
                    messageEntity.DateAut = messageEntity.UpdateDate;
                    messageEntity.UserPros = user;
                    messageEntity.DateProcess = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        StateBillCreate = billstateold,
                        StateBillUpdate = messageEntity.StateBill,
                        UserBillAutho = messageEntity.UserAut,
                        UserBillFinished = messageEntity.UserPros,
                        Observation = model.Transaction.Observation
                    };
                    string Factura = "\nSe cambia el estado de la factura de " + messagetransactionEntity.StateBillCreate.Name
                        + " por el estado " + messagetransactionEntity.StateBillUpdate.Name
                        + "\nse rechaza la factura por el usuario " + messagetransactionEntity.UserBillAutho.FullName
                        + " a las " + messageEntity.UpdateDateLocal
                        + "\nSe procesa la factura por el usuario " + messagetransactionEntity.UserBillFinished.FullName
                        + " a las " + messageEntity.UpdateDateLocal;
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                        + Factura
                        ;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha rechaza el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messageEntity.UserCreate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                    return RedirectToAction(nameof(Index));
                }
                //Operation 4 Proccess
                if (model.Operation == 4)
                {
                    #region Update Info
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                    messageEntity.UserPros = user;
                    messageEntity.DateProcess = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        StateBillCreate = billstateold,
                        StateBillUpdate = messageEntity.StateBill,
                        UserBillAutho = messageEntity.UserAut,
                        UserBillFinished = messageEntity.UserPros,
                        Observation = model.Transaction.Observation
                    };
                    string Factura = "\nSe cambia el estado de la factura de " + messagetransactionEntity.StateBillCreate.Name
                        + " por el estado " + messagetransactionEntity.StateBillUpdate.Name
                        + "\nSe procesa la factura por el usuario " + messagetransactionEntity.UserBillFinished.FullName
                        + " a las " + messageEntity.UpdateDateLocal;
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                        + Factura
                        ;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha procesado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messagetransactionEntity.UserUpdate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                    return RedirectToAction(nameof(Index));
                }
                //Operation 5 VO
                if (model.Operation == 5)
                {
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
                    modelEntity.UserSender = user;
                    modelEntity.User = await _userHelper.GetUserByIdAsync(model.UserRec);
                    _context.Update(modelEntity);
                    DateTime update = System.DateTime.Now.ToUniversalTime();
                    MessageCheckEntity check = new MessageCheckEntity
                    {
                        message = modelEntity,
                        User = user,
                        DateAut = messageEntity.UpdateDate
                    };
                    _context.Add(check);
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        StateCreate = modelEntity.State,
                        StateUpdate = modelEntity.State,
                        UpdateDate = update,
                        UserCreate = user,
                        UserUpdate = modelEntity.User,
                        Message = modelEntity,
                        Observation = model.Transaction.Observation
                    };
                    string Description = "";
                    Description += "Se da el visto bueno por el usuario " + user.FullName
                        + " en la fecha " + check.DateAutLocal;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsMessage), new { id = messageEntity.Id });
                }
                //Operation 6 Recibir Carta
                if (model.Operation == 6)
                {
                    #region Update Info
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Recibido");
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        Observation = model.Transaction.Observation
                    };
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha recibido el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messagetransactionEntity.UserUpdate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                    return RedirectToAction(nameof(Index));
                }
                //Operation 7 Recibir Paquete
                if (model.Operation == 7)
                {
                    #region Update Info
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Recibido");
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        Observation = model.Transaction.Observation
                    };
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha recibido el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messagetransactionEntity.UserUpdate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                    return RedirectToAction(nameof(Index));
                }
            }
            model.Type = await _context.MessagesTypes.FirstOrDefaultAsync(t => t.Id == model.TypeId);
            model.State = await _context.MessagesStates.FirstOrDefaultAsync(t => t.Id == model.StateId);
            model.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(t => t.Id == model.StateBillId);
            model.MessageType = _combosHelper.GetComboMessageType();
            model.Companies = _combosHelper.GetComboCompany();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.Users = _combosHelper.GetComboUser();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            model.Cecos = _combosHelper.GetComboCeCo(model.CompanyId);
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
                .Include(t => t.UserCreate)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.State)
                .Include(t => t.StateBill)
                .Include(t => t.Ceco)
                .Include(t => t.Company)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            UserEntity Userme = await _userHelper.GetUserAsync(User.Identity.Name);
            if (!messageEntity.User.Email.Equals(Userme.Email))
            {
                return View("../Account/NotAuthorized");
            }
            MessageViewModel messageViewModel = _converterHelper.ToMessageViewModel(messageEntity);
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
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                MessageEntity messageEntity = await _converterHelper.ToMessageEntityAsync(model, false);
                MessageBillStateEntity billstateold = _context.MessagesBillState.FirstOrDefault(s => s.Id == model.StateBillId);
                messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                messageEntity.UserSender = user;
                #region Load Files
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
                        Files += "\nEl usuario " + user.FullName
                            + " Agrega el archivo " + Nombre;
                    }
                }
                #endregion 
                //Operation 1 Edit
                if (model.Operation == 1)
                {
                    #region Update Info
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "En Proceso");
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        Observation = model.Transaction.Observation
                    };
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name;
                    if (messageEntity.Type.Name == "Factura")
                    {
                        messagetransactionEntity.StateBillCreate = billstateold;
                        messagetransactionEntity.StateBillUpdate = messageEntity.StateBill;
                        messagetransactionEntity.UserBillAutho = messageEntity.UserAut;
                        messagetransactionEntity.UserBillFinished = messageEntity.UserPros;
                        string Factura = "\nSe cambia el estado de la factura de " + messagetransactionEntity.StateBillCreate.Name
                        + " por el estado " + messagetransactionEntity.StateBillUpdate.Name;
                        Description += Factura;
                    }
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    if (messagetransactionEntity.UserCreate != messagetransactionEntity.UserUpdate)
                    {
                        string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                        string body =
                            "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                            " <br/> Hola. <br/> " +
                            "Se ha autorizado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                            messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                            "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                            "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                            "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                            "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                            "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                            "una respuesta. <br/> <br/> " +
                            "Atentamente,<br/>" +
                            "Equipo de Soporte - Refocosta.<br/>";
                        string[] to = new string[1];
                        to[0] = messagetransactionEntity.UserUpdate.Email;
                        _mailHelper.sendMail(to, subject, body);
                    }
                    #endregion
                    return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
                }
                //Operation 2 Authorize
                if (model.Operation == 2)
                {
                    #region Update Info
                    if (messageEntity.User == user)
                    {
                        if (messageEntity.Ceco.UserResponsible == null)
                        {
                            ViewBag.Mensaje = "Este centro de costos no cuenta con responsable. Seleccione el usuario de forma manual.";
                            model.Type = await _context.MessagesTypes.FirstOrDefaultAsync(t => t.Id == model.TypeId);
                            model.State = await _context.MessagesStates.FirstOrDefaultAsync(t => t.Id == model.StateId);
                            model.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(t => t.Id == model.StateBillId);
                            model.MessageType = _combosHelper.GetComboMessageType();
                            model.Companies = _combosHelper.GetComboCompany();
                            model.MessageState = _combosHelper.GetComboMessageState();
                            model.Users = _combosHelper.GetComboUser();
                            model.MessageBillState = _combosHelper.GetComboMessageBillState();
                            model.Cecos = _combosHelper.GetComboCeCo(model.CompanyId);
                            return View(model);
                        }
                        else
                        {
                            messageEntity.User = messageEntity.Ceco.UserResponsible;
                        }
                    }
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Aprobado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "En Proceso");
                    messageEntity.UserAut = user;
                    messageEntity.DateAut = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        StateBillCreate = billstateold,
                        StateBillUpdate = messageEntity.StateBill,
                        UserBillAutho = messageEntity.UserAut,
                        UserBillFinished = messageEntity.UserPros,
                        Observation = model.Transaction.Observation
                    };
                    string Factura = "\nSe cambia el estado de la factura de " + messagetransactionEntity.StateBillCreate.Name
                        + " por el estado " + messagetransactionEntity.StateBillUpdate.Name
                        + "\nse aprueba la factura por el usuario " + messagetransactionEntity.UserBillAutho.FullName
                        + " a las " + messageEntity.UpdateDateLocal;
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                        + Factura
                        ;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha autorizado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messagetransactionEntity.UserUpdate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                    return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
                }
                //Operation 3 Refuse
                if (model.Operation == 3)
                {
                    #region Update Info
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Rechazado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                    messageEntity.UserAut = user;
                    messageEntity.DateAut = messageEntity.UpdateDate;
                    messageEntity.UserPros = user;
                    messageEntity.DateProcess = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        StateBillCreate = billstateold,
                        StateBillUpdate = messageEntity.StateBill,
                        UserBillAutho = messageEntity.UserAut,
                        UserBillFinished = messageEntity.UserPros,
                        Observation = model.Transaction.Observation
                    };
                    string Factura = "\nSe cambia el estado de la factura de " + messagetransactionEntity.StateBillCreate.Name
                        + " por el estado " + messagetransactionEntity.StateBillUpdate.Name
                        + "\nse rechaza la factura por el usuario " + messagetransactionEntity.UserBillAutho.FullName
                        + " a las " + messageEntity.UpdateDateLocal
                        + "\nSe procesa la factura por el usuario " + messagetransactionEntity.UserBillFinished.FullName
                        + " a las " + messageEntity.UpdateDateLocal;
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                        + Factura
                        ;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha rechaza el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messageEntity.UserCreate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                    return RedirectToAction(nameof(IndexMe));
                }
                //Operation 4 Proccess
                if (model.Operation == 4)
                {
                    #region Update Info
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                    messageEntity.UserPros = user;
                    messageEntity.DateProcess = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        StateBillCreate = billstateold,
                        StateBillUpdate = messageEntity.StateBill,
                        UserBillAutho = messageEntity.UserAut,
                        UserBillFinished = messageEntity.UserPros,
                        Observation = model.Transaction.Observation
                    };
                    string Factura = "\nSe cambia el estado de la factura de " + messagetransactionEntity.StateBillCreate.Name
                        + " por el estado " + messagetransactionEntity.StateBillUpdate.Name
                        + "\nSe procesa la factura por el usuario " + messagetransactionEntity.UserBillFinished.FullName
                        + " a las " + messageEntity.UpdateDateLocal;
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                        + Factura
                        ;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha procesado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messagetransactionEntity.UserUpdate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                    return RedirectToAction(nameof(IndexMe));
                }
                //Operation 5 VO
                if (model.Operation == 5)
                {
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
                    modelEntity.UserSender = user;
                    modelEntity.User = await _userHelper.GetUserByIdAsync(model.UserRec);
                    _context.Update(modelEntity);
                    DateTime update = System.DateTime.Now.ToUniversalTime();
                    MessageCheckEntity check = new MessageCheckEntity
                    {
                        message = modelEntity,
                        User = user,
                        DateAut = messageEntity.UpdateDate
                    };
                    _context.Add(check);
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        StateCreate = modelEntity.State,
                        StateUpdate = modelEntity.State,
                        UpdateDate = update,
                        UserCreate = user,
                        UserUpdate = modelEntity.User,
                        Message = modelEntity,
                        Observation = model.Transaction.Observation
                    };
                    string Description = "";
                    Description += "Se da el visto bueno por el usuario " + user.FullName
                        + " en la fecha " + check.DateAutLocal;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
                }
                //Operation 6 Recibir Carta
                if (model.Operation == 6)
                {
                    #region Update Info
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Recibido");
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        Observation = model.Transaction.Observation
                    };
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha recibido el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messagetransactionEntity.UserUpdate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                    return RedirectToAction(nameof(IndexMe));
                }
                //Operation 7 Recibir Paquete
                if (model.Operation == 7)
                {
                    #region Update Info
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Recibido");
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = await _context.MessagesStates.FirstOrDefaultAsync(s => s.Id == model.StateIdOld),
                        StateUpdate = messageEntity.State,
                        Observation = model.Transaction.Observation
                    };
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name;
                    Description += Files;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha recibido el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messagetransactionEntity.UserUpdate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                    return RedirectToAction(nameof(IndexMe));
                }
            }
            model.Type = await _context.MessagesTypes.FirstOrDefaultAsync(t => t.Id == model.TypeId);
            model.State = await _context.MessagesStates.FirstOrDefaultAsync(t => t.Id == model.StateId);
            model.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(t => t.Id == model.StateBillId);
            model.MessageType = _combosHelper.GetComboMessageType();
            model.Companies = _combosHelper.GetComboCompany();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.Users = _combosHelper.GetComboUser();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            model.Cecos = _combosHelper.GetComboCeCo(model.CompanyId);
            return View(model);
        }
        #endregion
        #region Especial
        [Authorize(Roles = "Administrator,MessageAllAuthorize")]
        public async Task<IActionResult> MessageAuthorize()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            List<MessageEntity> list = await
                _context.Messages
                .Where(t => t.Company.Id == user.Company.Id)
                .Where(t => t.StateBill.Name == "Nuevo")
                .Where(t => t.User == user)
                .Where(t => t.Ceco != null)
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .Include(t => t.Company)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                ;
            List<MessageAutorizeViewModel> lista2 = new List<MessageAutorizeViewModel>();
            foreach (MessageEntity item in list)
            {
                lista2.Add(_converterHelper.ToMessageAutorizeViewModel(item));
            }
            if(lista2.Count>0)
            lista2[0].Users = _combosHelper.GetComboUserActive();


            return View(lista2);
        }
        [Authorize(Roles = "Administrator,MessageAllAuthorize")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MessageAuthorize(IEnumerable<MessageAutorizeViewModel> model)
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);

            var i = model.FirstOrDefault();
            string usuario = i.UserId;

            foreach (MessageAutorizeViewModel item in model)
            {
                if (item.Auto == true)
                {
                    MessageEntity messageEntity = new MessageEntity();
                    messageEntity = await _context.Messages
                       .Include(t => t.State)
                       .Include(t => t.User)
                       .Include(t => t.Type)
                       .Include(t => t.Ceco)
                       .ThenInclude(t => t.UserResponsible)
                       .Include(t => t.Company)
                       .Include(t => t.StateBill)
                       .FirstOrDefaultAsync(t => t.Id == item.Id);
                    MessageBillStateEntity billstateold = messageEntity.StateBill;
                    MessageStateEntity stateold = messageEntity.State;

                    #region Update Info

                    if (usuario == "0"|| usuario == null)
                    {
                        if (messageEntity.Ceco.UserResponsible == null)
                        {
                            //Enviar a responsable de facturacion de la empresa
                            messageEntity.User = user;
                        }
                        else
                        {
                            messageEntity.User = messageEntity.Ceco.UserResponsible;
                        }
                    }
                    else
                    {
                        messageEntity.User = await _userHelper.GetUserByIdAsync(usuario); 
                    }
                    messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                    messageEntity.UserSender = user;
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Aprobado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "En Proceso");
                    messageEntity.UserAut = user;
                    messageEntity.DateAut = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    #endregion
                    #region Create Transaction
                    MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity
                    {
                        Message = messageEntity,
                        UpdateDate = messageEntity.UpdateDateLocal.ToUniversalTime(),
                        UserCreate = messageEntity.UserSender,
                        UserUpdate = messageEntity.User,
                        StateCreate = stateold,
                        StateUpdate = messageEntity.State,
                        StateBillCreate = billstateold,
                        StateBillUpdate = messageEntity.StateBill,
                        UserBillAutho = user,
                        UserBillFinished = messageEntity.UserPros
                    };
                    string Factura = "\nSe cambia el estado de la factura de " + messagetransactionEntity.StateBillCreate.Name
                        + " por el estado " + messagetransactionEntity.StateBillUpdate.Name
                        + "\nse aprueba la factura por el usuario " + messagetransactionEntity.UserBillAutho.FullName
                        + " a las " + messageEntity.UpdateDateLocal;
                    string Description = "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " enviado al usuario  " + messagetransactionEntity.UserUpdate.FullName
                        + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                        + " y un estado final " + messagetransactionEntity.StateUpdate.Name
                        + Factura
                        ;
                    messagetransactionEntity.Description = Description;
                    _context.Add(messagetransactionEntity);
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                    string body =
                        "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                        " <br/> Hola. <br/> " +
                        "Se ha autorizado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                        messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
                        "Ingrese a http://nativa.refocosta.com para revisar el registro. <br/> <br/> " +
                        "Usted recibió este mensaje automático por que le fue asignado un documento, " +
                        "factura y / o paquete en el sistema de información NATIVA. Por favor ingrese con su " +
                        "usuario y contraseña para revisar y dale trámite al registro. Este mensaje es enviado " +
                        "desde una cuenta no gestionada, por lo tanto si usted contesta este correo no recibirá " +
                        "una respuesta. <br/> <br/> " +
                        "Atentamente,<br/>" +
                        "Equipo de Soporte - Refocosta.<br/>";
                    string[] to = new string[1];
                    to[0] = messagetransactionEntity.UserUpdate.Email;
                    _mailHelper.sendMail(to, subject, body);
                    #endregion
                }
            }


            List<MessageEntity> list = await
                _context.Messages
                .Where(t => t.Company.Id == user.Company.Id)
                .Where(t => t.StateBill.Name == "Nuevo")
                .Where(t => t.User == user)
                .Where(t => t.Ceco != null)
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.UserSender)
                .Include(t => t.MessageFiles)
                .Include(t => t.Ceco)
                .Include(t => t.Company)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                ;
            List<MessageAutorizeViewModel> lista2 = new List<MessageAutorizeViewModel>();
            foreach (MessageEntity item in list)
            {
                lista2.Add(_converterHelper.ToMessageAutorizeViewModel(item));
            }


            return View(lista2);
        }
        #endregion
    }
}