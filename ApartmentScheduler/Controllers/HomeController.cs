using ApartmentScheduler.Interfaces;
using ApartmentScheduler.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;
        private readonly IDataService _data;
        public HomeController(ILogger<HomeController> logger, INotyfService notyf, IDataService data)
        {
            _logger = logger;
            _notyf = notyf;
            _data = data;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signature()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult Register(string nick, string email, string password)
        {
            if (!string.IsNullOrEmpty(email)&& !string.IsNullOrEmpty(nick)&& !string.IsNullOrEmpty(password))
            {
                var result = _data.RegisterAsync(email,nick,password);
                if (result.Result.Equals("Success"))
                {
                    _notyf.Success("Welcome to our service");
                    TempData["user"] = nick;
                    return RedirectToAction("Index", "Logged");
                }
                if (result.Result.Equals("exists"))
                {
                    _notyf.Error("User with this email or nick name already exists");
                    return RedirectToAction(nameof(Index));
                }
                _notyf.Error(result.Result);
                return RedirectToAction(nameof(Index));
            }
            _notyf.Error("Fill all fields!");
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Login()
        {
            return PartialView();
        }
        [HttpGet]
        public IActionResult LoginPostShow(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var response = _data.LoginAsync(email, password);
                if (response.Result !=null)
                {
                    _notyf.Success("Login successfully");
                    TempData["user"] = response.Result.UserName.ToString();
                    return RedirectToAction("Index","Logged");
                }
                _notyf.Error("Incorrect email/password");
                return RedirectToAction(nameof(Index));
            }
            _notyf.Error("User or password is empty!");
            return RedirectToAction(nameof(Index));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
