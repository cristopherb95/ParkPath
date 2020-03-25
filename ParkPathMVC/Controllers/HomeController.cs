using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkPathMVC.Models;
using ParkPathMVC.Models.ViewModels;
using ParkPathMVC.Repository.IRepository;

namespace ParkPathMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepository;
        private readonly ITrailRepository _trailRepository;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository npRepository, ITrailRepository trailRepository)
        {
            _logger = logger;
            _npRepository = npRepository;
            _trailRepository = trailRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM viewModel = new IndexVM()
            {
                NationalParks = await _npRepository.GetAllAsync(SD.NationalParkAPIPath),
                Trails = await _trailRepository.GetAllAsync(SD.TrailAPIPath)
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
