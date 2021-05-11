using ApartmentScheduler.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Controllers
{
    public class LoggedController : Controller
    {
        private readonly IDataService _dataContext;
        public LoggedController(IDataService dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {

            var  a = TempData["user"] as string;
            ViewBag.test = a;
            return View();
        }
    }
}
