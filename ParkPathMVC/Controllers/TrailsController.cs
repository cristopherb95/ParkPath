using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkPathMVC.Models;
using ParkPathMVC.Models.ViewModels;
using ParkPathMVC.Repository.IRepository;

namespace ParkPathMVC.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepository;
        private readonly ITrailRepository _trailRepository;

        public TrailsController(ITrailRepository trailRepository, INationalParkRepository npRepository)
        {
            _trailRepository = trailRepository;
            _npRepository = npRepository;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> nationalParks = await _npRepository.GetAllAsync(SD.NationalParkAPIPath);

            TrailsVM trailsVM = new TrailsVM()
            {
                NationalParks = nationalParks.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Trail = new Trail()
            };

            if (id == null)
                return View(trailsVM);

            trailsVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());

            if (trailsVM.Trail == null)
                return NotFound();

            return View(trailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM trailVM)
        {
            if (ModelState.IsValid)
            {
                if (trailVM.Trail.Id == 0)
                {
                    await _trailRepository.CreateAsync(SD.TrailAPIPath, trailVM.Trail);
                }
                else
                {
                    await _trailRepository.UpdateAsync(SD.TrailAPIPath + trailVM.Trail.Id, trailVM.Trail);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> nationalParks = await _npRepository.GetAllAsync(SD.NationalParkAPIPath);

                TrailsVM objToReturn = new TrailsVM()
                {
                    NationalParks = nationalParks.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    Trail = trailVM.Trail
                };
                return View(objToReturn);
            }
        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new {data = await _trailRepository.GetAllAsync(SD.TrailAPIPath)});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteStatus = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (deleteStatus)
            {
                return Json(new {success = true, message = "Delete successful!"});
            }

            return Json(new {success = false, message = "Delete failed!"});
        }
        
    }
}