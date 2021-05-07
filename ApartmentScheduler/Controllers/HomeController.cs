﻿using ApartmentScheduler.Interfaces;
using ApartmentScheduler.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                _data.RegisterAsync(user);
                _notyf.Success("Welcome in our service");
                return RedirectToAction(nameof(Index));

            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return PartialView();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
