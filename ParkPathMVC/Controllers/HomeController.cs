using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
        private readonly IAccountRepository _accountRepository;
        private string Token => HttpContext.Session.GetString("JWToken");
        public HomeController(ILogger<HomeController> logger,
                            INationalParkRepository npRepository,
                            ITrailRepository trailRepository,
                            IAccountRepository accountRepository)
        {
            _logger = logger;
            _npRepository = npRepository;
            _trailRepository = trailRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM viewModel = new IndexVM()
            {
                NationalParks = await _npRepository.GetAllAsync(SD.NationalParkAPIPath, Token),
                Trails = await _trailRepository.GetAllAsync(SD.TrailAPIPath, Token)
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var loggedUser = await _accountRepository.LoginAsync(SD.AccountAPIPath + "authenticate/", user);

            if(loggedUser.Token == null)
            {
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, loggedUser.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, loggedUser.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
            HttpContext.Session.SetString("JWToken", loggedUser.Token);

            TempData["alert"] = "Welcome " + loggedUser.Username;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            var isRegisterSuccessful = await _accountRepository.RegisterAsync(SD.AccountAPIPath + "register/", user);

            if (!isRegisterSuccessful)
            {
                return View();
            }

            TempData["alert"] = "Registration successful!";
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Logout(User user)
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
