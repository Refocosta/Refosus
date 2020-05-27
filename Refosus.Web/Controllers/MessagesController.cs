using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System;
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
        public IActionResult CreateMessage()
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
        public async Task<IActionResult> CreateMessageAsync(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                MessageEntity messageEntity = new MessageEntity();
                messageEntity = await _converterHelper.ToMessageEntityAsync(model, true);
                messageEntity.CreateDate = System.DateTime.Now.ToUniversalTime();
                messageEntity.UpdateDate = System.DateTime.Now.ToUniversalTime();
                _context.Add(messageEntity);
                await _context.SaveChangesAsync();



                MessagetransactionEntity messagetransactionEntity = new MessagetransactionEntity();
                messagetransactionEntity = await _converterHelper.ToMessageTransactionEntityAsync(model);

                messagetransactionEntity.StateCreate = messageEntity.State ;
                messagetransactionEntity.StateUpdate = messageEntity.State;
                messagetransactionEntity.UpdateDate = messageEntity.UpdateDate;
                messagetransactionEntity.UserCreate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                messagetransactionEntity.UserUpdate = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                messagetransactionEntity.Message = messageEntity;


                _context.Add(messagetransactionEntity);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
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
                .Include(t => t.User)
                .Include(t => t.Transaction)
                .ThenInclude(t=>t.UserCreate)
                .Include(t => t.Transaction)
                .ThenInclude(t => t.StateCreate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            return View(messageEntity);
        }
    }
}