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
    public class CountriesController : Controller
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly ICombosHelper _combosHelper;

        public CountriesController(DataContext context, IConverterHelper converterHelper, ICombosHelper combosHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
            _combosHelper = combosHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context
                .Countries
                .Include(t => t.Departments)
                .OrderBy(t => t.Name)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CountryEntity conuntryEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conuntryEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conuntryEntity);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CountryEntity conuntryEntity = await _context.Countries.FindAsync(id);
            if (conuntryEntity == null)
            {
                return NotFound();
            }
            return View(conuntryEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CountryEntity conuntryEntity)
        {
            if (id != conuntryEntity.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Update(conuntryEntity);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(conuntryEntity);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CountryEntity conuntryEntity = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conuntryEntity == null)
            {
                return NotFound();
            }
            _context.Countries.Remove(conuntryEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CountryEntity countryEntity = await _context.Countries
                .Include(t => t.Departments)
                .ThenInclude(t => t.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (countryEntity == null)
            {
                return NotFound();
            }
            return View(countryEntity);
        }

        public async Task<IActionResult> AddDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CountryEntity countryEntity = await _context.Countries.FindAsync(id);
            if (countryEntity == null)
            {
                return NotFound();
            }
            DepartmentViewModel model = new DepartmentViewModel
            {
                Country = countryEntity,
                CountryId = countryEntity.Id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                DepartmentEntity deparmentEntity = await _converterHelper.ToDepartmentEntityAsync(model, true);
                _context.Add(deparmentEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new RouteValueDictionary(
    new { controller = "Countries", action = "Details", Id = model.CountryId }));
            }
            return View(model);
        }

        public async Task<IActionResult> EditDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DepartmentEntity departmentEntity = await _context.Deparments
                .Include(g => g.Country)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (departmentEntity == null)
            {
                return NotFound();
            }
            DepartmentViewModel model = _converterHelper.ToDepartmentViewModel(departmentEntity);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                DepartmentEntity departmentEntity = await _converterHelper.ToDepartmentEntityAsync(model, false);
                _context.Update(departmentEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new RouteValueDictionary(
                    new { controller = "Countries", action = "Details", Id = model.CountryId }));
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteDepartment(int? id)
        {
            if (id == null)
            {
                NotFound();
            }
            DepartmentEntity departmentEntity = await _context.Deparments
                .Include(g => g.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departmentEntity == null)
            {
                return NotFound();
            }
            _context.Deparments.Remove(departmentEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new RouteValueDictionary(
                    new { controller = "Countries", action = "Details", Id = departmentEntity.Country.Id }));
        }

        public async Task<IActionResult> DetailsDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DepartmentEntity deparmentEntity = await _context.Deparments
                .Include(g => g.Cities)
                .ThenInclude(g => g.Campus)
                .Include(g => g.Country)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (deparmentEntity == null)
            {
                return NotFound();
            }
            return View(deparmentEntity);
        }

        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DepartmentEntity departmentEntity = await _context.Deparments.FindAsync(id);
            if (departmentEntity == null)
            {
                return NotFound();
            }
            CityViewModel model = new CityViewModel
            {
                Department = departmentEntity,
                DepartmentId = departmentEntity.Id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                CityEntity cityEntity = await _converterHelper.ToCityEntityAsync(model, true);
                _context.Add(cityEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailsDepartment", new RouteValueDictionary(
    new { controller = "Countries", action = "DetailsDepartment", Id = model.DepartmentId }));
            }
            return View(model);
        }

        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CityEntity cityEntity = await _context.Cities
                .Include(g => g.Department)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (cityEntity == null)
            {
                return NotFound();
            }
            CityViewModel model = _converterHelper.ToCityViewModel(cityEntity);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                CityEntity cityEntity = await _converterHelper.ToCityEntityAsync(model, false);
                _context.Update(cityEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailsDepartment", new RouteValueDictionary(
                    new { controller = "Countries", action = "DetailsDepartment", Id = model.DepartmentId }));
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                NotFound();
            }
            CityEntity cityEntity = await _context.Cities
                .Include(g => g.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cityEntity == null)
            {
                return NotFound();
            }
            _context.Cities.Remove(cityEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsDepartment", new RouteValueDictionary(
                    new { controller = "Countries", action = "DetailsDepartment", Id = cityEntity.Department.Id }));
        }

        public async Task<IActionResult> DetailsCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CityEntity cityEntity = await _context.Cities
                .Include(g => g.Campus)
                .ThenInclude(g => g.CampusDetails)
                .Include(g => g.Department)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (cityEntity == null)
            {
                return NotFound();
            }
            return View(cityEntity);
        }

        public async Task<IActionResult> AddCampus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CityEntity cityEntity = await _context.Cities.FindAsync(id);
            if (cityEntity == null)
            {
                return NotFound();
            }
            CampusViewModel model = new CampusViewModel
            {
                City = cityEntity,
                CityId = cityEntity.Id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCampus(CampusViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = System.DateTime.Now.ToUniversalTime();
                CampusEntity campusEntity = await _converterHelper.ToCampusEntityAsync(model, true);
                _context.Add(campusEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailsCity", new RouteValueDictionary(
    new { controller = "Countries", action = "DetailsCity", Id = model.CityId }));
            }
            return View(model);
        }

        public async Task<IActionResult> EditCampus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CampusEntity campusEntity = await _context.Campus
                .Include(g => g.City)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (campusEntity == null)
            {
                return NotFound();
            }
            CampusViewModel model = _converterHelper.ToCampusViewModel(campusEntity);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCampus(CampusViewModel model)
        {
            if (ModelState.IsValid)
            {
                CampusEntity campusEntity = await _converterHelper.ToCampusEntityAsync(model, false);
                _context.Update(campusEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailsCity", new RouteValueDictionary(
                    new { controller = "Countries", action = "DetailsCity", Id = model.CityId }));
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteCampus(int? id)
        {
            if (id == null)
            {
                NotFound();
            }
            CampusEntity campusEntity = await _context.Campus
                .Include(g => g.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campusEntity == null)
            {
                return NotFound();
            }
            _context.Campus.Remove(campusEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsCity", new RouteValueDictionary(
                    new { controller = "Countries", action = "DetailsCity", Id = campusEntity.City.Id }));
        }

        public async Task<IActionResult> DetailsCampus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CampusEntity campusEntity = await _context.Campus
                .Include(g => g.CampusDetails)
                .ThenInclude(g => g.Company)
                .Include(g => g.City)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (campusEntity == null)
            {
                return NotFound();
            }
            return View(campusEntity);
        }

        public async Task<IActionResult> AddCampusDetails(int? id)
        {
            if (id == null)
            {
                NotFound();
            }
            CampusEntity campusEntity = await _context.Campus.FindAsync(id);
            if (campusEntity == null)
            {
                return NotFound();
            }
            CampusDetailsViewModel model = new CampusDetailsViewModel
            {
                Campus = campusEntity,
                CampusId = campusEntity.Id,
                Companies = _combosHelper.GetComboCompany()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCampusDetails(CampusDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                CampusDetailsEntity campusDetailsEntity = await _converterHelper.ToCampusDetailsEntityAsync(model, true);
                _context.Add(campusDetailsEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailsCampus", new RouteValueDictionary(
                new { controller = "Countries", action = "DetailsCampus", Id = model.CampusId }));
            }
            model.Campus = await _context.Campus.FindAsync(model.CampusId);
            model.Companies = _combosHelper.GetComboCompany();
            return View(model);
        }

        public async Task<IActionResult> DeleteCampusDetails(int? id)
        {
            if (id == null)
            {
                NotFound();
            }
            CampusDetailsEntity campusDetailsEntity = await _context.CampusDetails
                .Include(g => g.Campus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campusDetailsEntity == null)
            {
                return NotFound();
            }
            _context.CampusDetails.Remove(campusDetailsEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsCampus", new RouteValueDictionary(
                    new { controller = "Countries", action = "DetailsCampus", Id = campusDetailsEntity.Campus.Id }));
        }

    }
}
