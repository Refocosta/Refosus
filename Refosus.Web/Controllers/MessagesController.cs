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
        //OK 29-09-2020 11:20
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
                .Include(t => t.Company)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        [Authorize(Roles = "Administrator,MessageMeMessage")]
        public async Task<IActionResult> IndexMeAsync()
        {
            UserEntity Userme = await _userHelper.GetUserAsync(User.Identity.Name);
            return View(await _context
                .Messages
                .Where(t => (t.User.Id == Userme.Id && t.State.Name != "Tramitado") || ((t.UserSender.Id == Userme.Id && t.State.Name != "Tramitado")))
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
                .Where(t => (t.User.Id == Userme.Id && t.State.Name == "Tramitado") || ((t.UserSender.Id == Userme.Id && t.State.Name == "Tramitado")))
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
                .Include(t => t.Company)
                .Include(t => t.StateBill)
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
                .Include(t => t.Company)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        #endregion
        //OK 29-09-2020 11:20
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
                MessageEntity messageEntity = new MessageEntity();
                messageEntity = await _converterHelper.ToMessageEntityAsync(model, true);
                messageEntity.CreateDate = System.DateTime.Now.ToUniversalTime();
                messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();

                if (messageEntity.User == null)
                {
                    messageEntity.User = await _userHelper.GetUserAsync(User.Identity.Name);
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
                messageEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
                messageEntity.UserSender = await _userHelper.GetUserAsync(User.Identity.Name);
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
                messagetransactionEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
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
                int id = messageEntity.Id;
                string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                string body =
                    "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                    " <br/> Hola. <br/> " +
                    "Se ha creado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                    messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
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
                return RedirectToAction(nameof(DetailsMessage), new { id });
            }
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            model.Users = _combosHelper.GetComboUser();
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
                MessageEntity messageEntity = new MessageEntity();
                messageEntity = await _converterHelper.ToMessageEntityAsync(model, true);
                messageEntity.CreateDate = System.DateTime.Now.ToUniversalTime();
                messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();

                if (messageEntity.User == null)
                {
                    messageEntity.User = await _userHelper.GetUserAsync(User.Identity.Name);
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
                messageEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
                messageEntity.UserSender = await _userHelper.GetUserAsync(User.Identity.Name);
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
                messagetransactionEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
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

                int id = messageEntity.Id;
                string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                string body =
                    "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                    " <br/> Hola. <br/> " +
                    "Se ha creado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                    messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
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

                return RedirectToAction(nameof(DetailsMeMessage), new { id });

            }
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            model.Users = _combosHelper.GetComboUser();
            return View(model);
        }
        #endregion
        //OK 29-09-2020 11:20
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
            UserEntity Userme = await _userHelper.GetUserAsync(User.Identity.Name);
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
                .Include(t => t.Company)
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
                UserEntity user = new UserEntity();
                user = await _userHelper.GetUserAsync(User.Identity.Name.ToString());
                MessageEntity messageEntity = await _converterHelper.ToMessageEntityAsync(model, false);
                messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                //Operation 1 Edit
                if (model.Operation == 1)
                {

                    if (messageEntity.User != await _userHelper.GetUserAsync(User.Identity.Name))
                    {
                        if (messageEntity.User == messageEntity.UserSender)
                        {
                            messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Ingresado");
                        }
                        else
                        {
                            messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "En Proceso");
                        }
                    }
                    messageEntity.UserSender = user;
                    _context.Update(messageEntity);
                    #region Create Transaction
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
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
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
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    if (messagetransactionEntity.UserCreate != messagetransactionEntity.UserUpdate)
                    {
                        string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                        string body =
                            "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                            " <br/> Hola. <br/> " +
                            "Se ha asignado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                            messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
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
                    }
                    #endregion
                    return RedirectToAction(nameof(DetailsMessage), new { id = messageEntity.Id });
                }
                //Operation 2 Authorize
                if (model.Operation == 2)
                {
                    if (messageEntity.User != await _userHelper.GetUserAsync(User.Identity.Name))
                    {
                        if (messageEntity.User == messageEntity.UserSender)
                        {
                            messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Ingresado");
                            messageEntity.UserSender = await _userHelper.GetUserAsync(User.Identity.Name);
                        }
                        else
                        {
                            messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "En Proceso");
                            messageEntity.UserSender = await _userHelper.GetUserAsync(User.Identity.Name);
                        }
                    }

                    MessageBillStateEntity billstateold = _context.MessagesBillState.FirstOrDefault(s => s.Id == model.StateBillId);
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Aprobado");
                    messageEntity.UserAut = user;
                    messageEntity.DateAut = messageEntity.UpdateDate;
                    messageEntity.UserSender = user;
                    _context.Update(messageEntity);
                    model.UserAut = await _userHelper.GetUserAsync(User.Identity.Name);

                    string Factura = " se cambia el estado de la factura de " + billstateold.Name
                        + " por el estado " + _context.MessagesBillState.FirstOrDefault(s => s.Id == messageEntity.StateBill.Id).Name
                        + " se autoriza la factura por el usuario " + messageEntity.UserAut.FullName
                        + " a las " + messageEntity.UpdateDateLocal
                        ;
                    _context.Update(messageEntity);
                    #region Create Transaction
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
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
                    messagetransactionEntity.UserUpdate = messageEntity.User;
                    messagetransactionEntity.Message = messageEntity;
                    string Description = "";
                    Description += "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
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
                    if (messagetransactionEntity.UserCreate != messagetransactionEntity.UserUpdate)
                    {
                        string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                        string body =
                            "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                            " <br/> Hola. <br/> " +
                            "Se ha asignado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                            messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
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
                    }
                    #endregion
                    return RedirectToAction(nameof(DetailsMessage), new { id = messageEntity.Id });
                }
                //Operation 3 Refuse
                if (model.Operation == 3)
                {
                    MessageBillStateEntity billstateold = _context.MessagesBillState.FirstOrDefault(s => s.Id == model.StateBillId);
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                    messageEntity.UserAut = user;
                    messageEntity.DateAut = messageEntity.UpdateDate;
                    messageEntity.UserPros = user;
                    messageEntity.DateProcess = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    model.UserAut = await _userHelper.GetUserAsync(User.Identity.Name);

                    string Factura = " se cambia el estado de la factura de " + billstateold.Name
                        + " por el estado " + _context.MessagesBillState.FirstOrDefault(s => s.Id == messageEntity.StateBill.Id).Name
                        + " se autoriza la factura por el usuario " + messageEntity.UserAut.FullName
                        + " a las " + messageEntity.UpdateDateLocal
                        ;
                    _context.Update(messageEntity);
                    #region Create Transaction
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
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
                    messagetransactionEntity.UserUpdate = messageEntity.User;
                    messagetransactionEntity.Message = messageEntity;
                    string Description = "";
                    Description += "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
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
                    if (messagetransactionEntity.UserCreate != messagetransactionEntity.UserUpdate)
                    {
                        string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                        string body =
                            "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                            " <br/> Hola. <br/> " +
                            "Se ha asignado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                            messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
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
                    }
                    #endregion
                    return RedirectToAction(nameof(DetailsMessage), new { id = messageEntity.Id });
                }
                //Operation 4 Proccess
                if (model.Operation == 4)
                {
                    MessageBillStateEntity billstateold = _context.MessagesBillState.FirstOrDefault(s => s.Id == model.StateBillId);
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");

                    messageEntity.UserPros = user;
                    messageEntity.DateProcess = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    model.UserAut = await _userHelper.GetUserAsync(User.Identity.Name);

                    string Factura = " se cambia el estado de la factura de " + billstateold.Name
                        + " por el estado " + _context.MessagesBillState.FirstOrDefault(s => s.Id == messageEntity.StateBill.Id).Name
                        + " se procesa la factura por el usuario " + messageEntity.UserPros.FullName
                        + " a las " + messageEntity.UpdateDateLocal
                        ;
                    _context.Update(messageEntity);
                    #region Create Transaction
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
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
                    messagetransactionEntity.UserUpdate = messageEntity.User;
                    messagetransactionEntity.Message = messageEntity;
                    string Description = "";
                    Description += "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
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
                    if (messagetransactionEntity.UserCreate != messagetransactionEntity.UserUpdate)
                    {
                        string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                        string body =
                            "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                            " <br/> Hola. <br/> " +
                            "Se ha asignado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                            messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
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
                    }
                    #endregion
                    return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
                }
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

                    modelEntity.User = await _userHelper.GetUserByIdAsync(model.CreateUser);
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

            }
            model.Type = await _context.MessagesTypes.FirstOrDefaultAsync(t => t.Id == model.TypeId);
            model.State = await _context.MessagesStates.FirstOrDefaultAsync(t => t.Id == model.StateId);
            model.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(t => t.Id == model.StateBillId);
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.Users = _combosHelper.GetComboUser();
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
                UserEntity user = new UserEntity();
                user = await _userHelper.GetUserAsync(User.Identity.Name.ToString());
                MessageEntity messageEntity = await _converterHelper.ToMessageEntityAsync(model, false);
                messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                //Operation 1 Edit
                if (model.Operation == 1)
                {

                    if (messageEntity.User != await _userHelper.GetUserAsync(User.Identity.Name))
                    {
                        if (messageEntity.Type.Name != "Paquete")
                            if (messageEntity.User == messageEntity.UserSender)
                            {
                                messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Ingresado");
                            }
                            else
                            {
                                messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "En Proceso");
                            }
                    }
                    messageEntity.UserSender = user;
                    _context.Update(messageEntity);
                    #region Create Transaction
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
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
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
                    #endregion
                    await _context.SaveChangesAsync();
                    #region Send email
                    if (messagetransactionEntity.UserCreate != messagetransactionEntity.UserUpdate)
                    {
                        string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                        string body =
                            "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                            " <br/> Hola. <br/> " +
                            "Se ha asignado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                            messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
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
                    }
                    #endregion
                    return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
                }
                //Operation 2 Authorize
                if (model.Operation == 2)
                {
                    if (messageEntity.User != await _userHelper.GetUserAsync(User.Identity.Name))
                    {
                        if (messageEntity.User == messageEntity.UserSender)
                        {
                            messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Ingresado");
                            messageEntity.UserSender = await _userHelper.GetUserAsync(User.Identity.Name);
                        }
                        else
                        {
                            messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "En Proceso");
                            messageEntity.UserSender = await _userHelper.GetUserAsync(User.Identity.Name);
                        }
                    }

                    MessageBillStateEntity billstateold = _context.MessagesBillState.FirstOrDefault(s => s.Id == model.StateBillId);
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Aprobado");
                    messageEntity.UserAut = user;
                    messageEntity.DateAut = messageEntity.UpdateDate;
                    messageEntity.UserSender = user;
                    _context.Update(messageEntity);
                    model.UserAut = await _userHelper.GetUserAsync(User.Identity.Name);

                    string Factura = " se cambia el estado de la factura de " + billstateold.Name
                        + " por el estado " + _context.MessagesBillState.FirstOrDefault(s => s.Id == messageEntity.StateBill.Id).Name
                        + " se autoriza la factura por el usuario " + messageEntity.UserAut.FullName
                        + " a las " + messageEntity.UpdateDateLocal
                        ;
                    _context.Update(messageEntity);
                    #region Create Transaction
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
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
                    messagetransactionEntity.UserUpdate = messageEntity.User;
                    messagetransactionEntity.Message = messageEntity;
                    string Description = "";
                    Description += "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
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
                    if (messagetransactionEntity.UserCreate != messagetransactionEntity.UserUpdate)
                    {
                        string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                        string body =
                            "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                            " <br/> Hola. <br/> " +
                            "Se ha asignado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                            messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
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
                    }
                    #endregion
                    return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
                }
                //Operation 3 Refuse
                if (model.Operation == 3)
                {
                    MessageBillStateEntity billstateold = _context.MessagesBillState.FirstOrDefault(s => s.Id == model.StateBillId);
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
                    messageEntity.UserAut = user;
                    messageEntity.DateAut = messageEntity.UpdateDate;
                    messageEntity.UserPros = user;
                    messageEntity.DateProcess = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    model.UserAut = await _userHelper.GetUserAsync(User.Identity.Name);

                    string Factura = " se cambia el estado de la factura de " + billstateold.Name
                        + " por el estado " + _context.MessagesBillState.FirstOrDefault(s => s.Id == messageEntity.StateBill.Id).Name
                        + " se autoriza la factura por el usuario " + messageEntity.UserAut.FullName
                        + " a las " + messageEntity.UpdateDateLocal
                        ;
                    _context.Update(messageEntity);
                    #region Create Transaction
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
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
                    messagetransactionEntity.UserUpdate = messageEntity.User;
                    messagetransactionEntity.Message = messageEntity;
                    string Description = "";
                    Description += "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
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
                    if (messagetransactionEntity.UserCreate != messagetransactionEntity.UserUpdate)
                    {
                        string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                        string body =
                            "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                            " <br/> Hola. <br/> " +
                            "Se ha asignado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                            messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
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
                    }
                    #endregion
                    return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
                }
                //Operation 4 Proccess
                if (model.Operation == 4)
                {
                    MessageBillStateEntity billstateold = _context.MessagesBillState.FirstOrDefault(s => s.Id == model.StateBillId);
                    messageEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
                    messageEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");

                    messageEntity.UserPros = user;
                    messageEntity.DateProcess = messageEntity.UpdateDate;
                    _context.Update(messageEntity);
                    model.UserAut = await _userHelper.GetUserAsync(User.Identity.Name);

                    string Factura = " se cambia el estado de la factura de " + billstateold.Name
                        + " por el estado " + _context.MessagesBillState.FirstOrDefault(s => s.Id == messageEntity.StateBill.Id).Name
                        + " se procesa la factura por el usuario " + messageEntity.UserPros.FullName
                        + " a las " + messageEntity.UpdateDateLocal
                        ;
                    _context.Update(messageEntity);
                    #region Create Transaction
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
                    messagetransactionEntity.UserCreate = await _userHelper.GetUserAsync(User.Identity.Name);
                    messagetransactionEntity.UserUpdate = messageEntity.User;
                    messagetransactionEntity.Message = messageEntity;
                    string Description = "";
                    Description += "Se actualiza el mensaje de tipo " + messageEntity.Type.Name
                        + " en la fecha " + messageEntity.UpdateDateLocal
                        + " por el usuario " + messagetransactionEntity.UserCreate.FullName
                        + " dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
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
                    if (messagetransactionEntity.UserCreate != messagetransactionEntity.UserUpdate)
                    {
                        string subject = "Correspondencia No. " + messageEntity.Id + " - " + messageEntity.Reference;
                        string body =
                            "Mensaje enviado automáticamente por Nativa - Módulo de Correspondencia.Por favor no responda este mensaje. <br/>" +
                            " <br/> Hola. <br/> " +
                            "Se ha asignado el registro de tipo <strong>" + messageEntity.Type.Name + "</strong>, <strong>" + messageEntity.Reference + "</strong> con número de radicado <strong>" +
                            messageEntity.Id + ".</strong> Recibido por <strong>" + messagetransactionEntity.UserCreate.FullName + "</strong> y asignado a <strong>" + messagetransactionEntity.UserUpdate.FullName + ".</strong> <br/> " +
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
                    }
                    #endregion
                    return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
                }
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

                    modelEntity.User = await _userHelper.GetUserByIdAsync(model.CreateUser);
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
            }
            model.Type = await _context.MessagesTypes.FirstOrDefaultAsync(t => t.Id == model.TypeId);
            model.State = await _context.MessagesStates.FirstOrDefaultAsync(t => t.Id == model.StateId);
            model.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(t => t.Id == model.StateBillId);
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.Users = _combosHelper.GetComboUser();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            model.Cecos = _combosHelper.GetComboCeCo(model.CompanyId);
            model.Companies = _combosHelper.GetComboCompany();
            return View(model);
        }

        #endregion




        #region Message
        [Authorize(Roles = "Administrator,MessageAdministrator")]
        public async Task<IActionResult> ReceiveMessageAsync(int? id, string note)
        {
            if (note == null)
                note = "";
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
                UserCreate = await _userHelper.GetUserAsync(User.Identity.Name),
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

        #endregion

        #region MeMessage
        [Authorize(Roles = "Administrator,MessageMeMessage")]
        public async Task<IActionResult> ReceiveMeMessageAsync(int? id, string note)
        {
            if (note == null)
                note = "";
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
                UserCreate = await _userHelper.GetUserAsync(User.Identity.Name),
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
        #endregion

    }
}