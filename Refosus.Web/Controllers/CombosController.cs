using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Helpers;

namespace Refosus.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombosController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly CombosHelper _combosHelper;

        public CombosController(DbContext context,CombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        //// GET: api/Combos
        //[HttpGet]
        //public async Task<string> getCombosService(int UBP)
        //{
        //    var response = await _context. .FirstOrDefaultAsync(p => p.Id == UBP).Result.IdUserProjectBoss;
        //    return response;


        //}

        //public async Task<string> UserProjectBoss(int UBP)
        //{
        //    if (UBP != 0)
        //        return _context.Projects.FirstOrDefaultAsync(p => p.Id == UBP).Result.IdUserProjectBoss;
        //    return null;
        //}



    }
}