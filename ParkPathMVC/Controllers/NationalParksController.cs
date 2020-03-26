using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkPathMVC.Models;
using ParkPathMVC.Repository.IRepository;

namespace ParkPathMVC.Controllers
{
    [Authorize]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepository;
        private string Token => HttpContext.Session.GetString("JWToken");
        
        public NationalParksController(INationalParkRepository npRepository)
        {
            _npRepository = npRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark nationalPark = new NationalPark();

            if (id == null)
                return View(nationalPark);

            nationalPark = await _npRepository.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault(), Token);

            if (nationalPark == null)
                return NotFound();

            return View(nationalPark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    nationalPark.Picture = SD.GetByteArrayFromImage(files[0]);
                }
                else
                {
                    var npFromDb = await _npRepository.GetAsync(SD.NationalParkAPIPath, nationalPark.Id, Token);
                    nationalPark.Picture = npFromDb.Picture;
                }

                if (nationalPark.Id == 0)
                {
                    await _npRepository.CreateAsync(SD.NationalParkAPIPath, nationalPark, Token);
                }
                else
                {
                    await _npRepository.UpdateAsync(SD.NationalParkAPIPath + nationalPark.Id, nationalPark, Token);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nationalPark);
            }
        }
        
        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(new {data = await _npRepository.GetAllAsync(SD.NationalParkAPIPath, Token)});
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteStatus = await _npRepository.DeleteAsync(SD.NationalParkAPIPath, id, Token);
            if (deleteStatus)
            {
                return Json(new {success = true, message = "Delete successful!"});
            }
            return Json(new {success = false, message = "Delete failed!"});
        }
        
    }
}