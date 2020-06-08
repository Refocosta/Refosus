using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{
    public class MessagesController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public MessagesController(DataContext context,
            IConverterHelper converterHelper,
            ICombosHelper combosHelper,
            IUserHelper userHelper
            )
        {
            _context = context;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }
        #region Message
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context
                .Messages
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMessageAsync(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                string Description = "";
                MessageEntity messageEntity = new MessageEntity();

                messageEntity = await _converterHelper.ToMessageEntityAsync(model, true);
                messageEntity.CreateDate = System.DateTime.Now.ToUniversalTime();
                messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync(o => o.Name == "Ingresado");
                if (messageEntity.User == null)
                {
                    messageEntity.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                }
                if (messageEntity.Type.Name == "Factura")
                {
                    messageEntity.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(o => o.Name == "Nuevo");
                }
                else
                {
                    messageEntity.StateBill = await _context.MessagesBillState.FirstOrDefaultAsync(o => o.Name == "Otro");
                }
                _context.Add(messageEntity);




                MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
                messagetransactionEntity = await _converterHelper.ToMessageTransactionEntityAsync(model);
                messagetransactionEntity.StateCreate = messageEntity.State;
                messagetransactionEntity.StateUpdate = messageEntity.State;
                messagetransactionEntity.UpdateDate = messageEntity.UpdateDate;
                messagetransactionEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                messagetransactionEntity.UserUpdate = messageEntity.User;
                messagetransactionEntity.Message = messageEntity;



                Description += "Se crea el mensaje de tipo " + messageEntity.Type.Name
                    + " a las " + messageEntity.CreateDate
                    + " Por el usuario " + messagetransactionEntity.UserCreate.FullName
                    + " Dirigido al usuario  " + messagetransactionEntity.UserUpdate.FullName
                    + " con un estado inicial " + messagetransactionEntity.StateCreate.Name
                    + " con un estado Final " + messagetransactionEntity.StateUpdate.Name
                    ;
                messagetransactionEntity.Description = Description;


                _context.Add(messagetransactionEntity);



                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(DetailsMessage), new { id = messageEntity.Id });

            }




            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.Users = _combosHelper.GetComboActiveUser();
            return View(model);
        }
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
                .Include(t => t.Transaction)
                .ThenInclude(t => t.UserCreate)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.StateCreate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            return View(messageEntity);
        }
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
                _context.Update(messageEntity);

                MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();

                messagetransactionEntity = await _converterHelper.ToMessageTransactionEntityAsync(model);

                messagetransactionEntity.StateCreate = _context.MessagesStates.FirstOrDefault(g => g.Id == model.StateIdOld);
                messagetransactionEntity.StateUpdate = messageEntity.State;
                messagetransactionEntity.UpdateDate = messageEntity.UpdateDate;
                messagetransactionEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                messagetransactionEntity.UserUpdate = messageEntity.User;
                messagetransactionEntity.Message = messageEntity;


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
        public async Task<IActionResult> ReceiveMessageAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            if (note == null)
            {
                return RedirectToAction("EditMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "EditMessage", Id = id }));
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
            messagetransactionEntity.Message = modelEntity;
            messagetransactionEntity.UserCreate = modelEntity.User;
            messagetransactionEntity.UserUpdate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            messagetransactionEntity.StateCreate = modelEntity.State;
            modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "EnProceso");
            messagetransactionEntity.StateUpdate = modelEntity.State;
            messagetransactionEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            messagetransactionEntity.Observation = note;
            modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Recibido");
            if (modelEntity == null)
            {
                return NotFound();
            }
            _context.Update(modelEntity);
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("EditMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "EditMessage", Id = modelEntity.Id }));
        }
        public async Task<IActionResult> AuthorizeMessageAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            if (note == null)
            {
                return RedirectToAction("EditMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "EditMessage", Id = id }));
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
            messagetransactionEntity.Message = modelEntity;
            messagetransactionEntity.UserCreate = modelEntity.User;
            messagetransactionEntity.UserUpdate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            messagetransactionEntity.StateCreate = modelEntity.State;
            messagetransactionEntity.StateUpdate = modelEntity.State;
            messagetransactionEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            messagetransactionEntity.Observation = note;
            modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Aprobado");
            modelEntity.UserAut = messagetransactionEntity.UserUpdate;
            modelEntity.DateAut = System.DateTime.Now.ToUniversalTime();
            if (modelEntity == null)
            {
                return NotFound();
            }
            _context.Update(modelEntity);
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("EditMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "EditMessage", Id = modelEntity.Id }));
        }
        public async Task<IActionResult> RefuseMessageAsync(int? id, string note)
        {
            if (id == null)
            {
                NotFound();
            }
            if (note == null)
            {
                return RedirectToAction("EditMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "EditMessage", Id = id }));
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
            messagetransactionEntity.Message = modelEntity;
            messagetransactionEntity.UserCreate = modelEntity.User;
            messagetransactionEntity.UserUpdate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            messagetransactionEntity.StateCreate = modelEntity.State;
            modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
            messagetransactionEntity.StateUpdate = modelEntity.State;
            messagetransactionEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            messagetransactionEntity.Observation = note;
            modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Rechazado");
            modelEntity.UserAut = messagetransactionEntity.UserUpdate;
            modelEntity.DateAut = System.DateTime.Now.ToUniversalTime();
            modelEntity.UserPros = messagetransactionEntity.UserUpdate;
            modelEntity.DateProcess = System.DateTime.Now.ToUniversalTime();
            
            if (modelEntity == null)
            {
                return NotFound();
            }
            _context.Update(modelEntity);
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
            if (note == null)
            {
                return RedirectToAction("EditMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "EditMessage", Id = id }));
            }
            MessageEntity modelEntity = await _context.Messages
                .Include(o => o.State)
                .Include(o => o.StateBill)
                .Include(o => o.User)
                .Include(o => o.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
            messagetransactionEntity.Message = modelEntity;
            messagetransactionEntity.UserCreate = modelEntity.User;
            messagetransactionEntity.UserUpdate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            messagetransactionEntity.StateCreate = modelEntity.State;
            modelEntity.State = _context.MessagesStates.FirstOrDefault(o => o.Name == "Tramitado");
            messagetransactionEntity.StateUpdate = modelEntity.State;
            messagetransactionEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
            messagetransactionEntity.Observation = note;
            modelEntity.StateBill = _context.MessagesBillState.FirstOrDefault(o => o.Name == "Procesado");
            modelEntity.UserPros = messagetransactionEntity.UserUpdate;
            modelEntity.DateProcess = System.DateTime.Now.ToUniversalTime();
            if (modelEntity == null)
            {
                return NotFound();
            }
            _context.Update(modelEntity);
            _context.Add(messagetransactionEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("EditMessage", new RouteValueDictionary(
                    new { controller = "Messages", action = "EditMessage", Id = modelEntity.Id }));
        }
        #endregion
        #region MeMessage
        public async Task<IActionResult> IndexMeAsync()
        {
            UserEntity Userme = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            return View(await _context
                .Messages
                .Where(t => t.User.Id == Userme.Id)
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .OrderBy(t => t.UpdateDate)
                .ToListAsync()
                );
        }
        public IActionResult CreateMeMessage()
        {
            MessageViewModel model = new MessageViewModel
            {
                MessageType = _combosHelper.GetComboMessageType(),
                MessageState = _combosHelper.GetComboMessageState(),
                Users = _combosHelper.GetComboActiveUser()
            };
            return View(model);
        }

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
                messageEntity.State = await _context.MessagesStates.FirstOrDefaultAsync();
                if (messageEntity.User == null)
                {
                    messageEntity.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                }
                _context.Add(messageEntity);
                await _context.SaveChangesAsync();

                MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
                messagetransactionEntity = await _converterHelper.ToMessageTransactionEntityAsync(model);

                messagetransactionEntity.StateCreate = messageEntity.State;
                messagetransactionEntity.StateUpdate = messageEntity.State;
                messagetransactionEntity.UpdateDate = messageEntity.UpdateDate;
                messagetransactionEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                messagetransactionEntity.UserUpdate = messageEntity.User;
                messagetransactionEntity.Message = messageEntity;


                _context.Add(messagetransactionEntity);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });

            }
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.Users = _combosHelper.GetComboActiveUser();
            return View(model);
        }
        public async Task<IActionResult> DetailsMeMessage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MessageEntity messageEntity = await _context.Messages
                .Include(t => t.Type)
                .Include(t => t.State)
                .Include(t => t.User)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.UserCreate)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.StateCreate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            return View(messageEntity);
        }
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
                .FirstOrDefaultAsync(g => g.Id == id);
            UserEntity Userme = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (!messageEntity.User.Email.Equals(Userme.Email))
            {
                return View("../Account/NotAuthorized");
            }
            if (messageEntity == null)
            {
                return NotFound();
            }
            MessageViewModel messageViewModel = _converterHelper.ToMessageViewModel(messageEntity);
            return View(messageViewModel);
        }

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

                MessageEntity messageEntity = await _converterHelper.ToMessageEntityAsync(model, false);
                messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Update(messageEntity);

                MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
                messagetransactionEntity = await _converterHelper.ToMessageTransactionEntityAsync(model);

                messagetransactionEntity.StateCreate = _context.MessagesStates.FirstOrDefault(g => g.Id == model.StateIdOld);
                messagetransactionEntity.StateUpdate = messageEntity.State;
                messagetransactionEntity.UpdateDate = messageEntity.UpdateDate;
                messagetransactionEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                messagetransactionEntity.UserUpdate = messageEntity.User;
                messagetransactionEntity.Message = messageEntity;


                _context.Add(messagetransactionEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsMeMessage), new { id = messageEntity.Id });
            }
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.Users = _combosHelper.GetComboActiveUser();
            return View(model);
        }

        #endregion
        #region Transaction
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
        #endregion
        #region MeTransaction
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
            UserEntity Userme = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (messagetransaction == null)
            {
                return NotFound();
            }
            return View(messagetransaction);
        }
        #endregion



















    }
}