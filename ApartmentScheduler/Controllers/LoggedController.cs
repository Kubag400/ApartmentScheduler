using ApartmentScheduler.Interfaces;
using ApartmentScheduler.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApartmentScheduler.Controllers
{
    public class LoggedController : Controller
    {
        private readonly IDataService _data;
        public static IdentityUser _user;
        private readonly INotyfService _notyf;

        public LoggedController(IDataService data, INotyfService notyf)
        {
            _data = data;
            _notyf = notyf;
        }
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                _user = _data.GetUserAsync(TempData["user"] as string).Result;
                ViewBag.User = _user;
                return View();
            }
            catch
            {
                _notyf.Warning("Login once again");
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetApartments()
        {

            var vM = new GetApartmentsViewModel
            {
                Apartments = await _data.GetApartmentsAsync(_user.Id),
                Contributions = await _data.GetUserContributionsAsync(_user.Id)
            };
            return PartialView(vM);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, int kitchen, int toilet, int room)
        {
            if (ModelState.IsValid)
            {
                var apartment = new Apartment
                {
                    Name = name,
                    Kitchen = kitchen,
                    Toilet = toilet,
                    Room = room
                };
                var addApartment = await _data.CreateApartmentAsync(apartment, _user.UserName);
                if (addApartment == true)
                {
                    _notyf.Success("Apartment added successfully!");
                    //return RedirectToAction(nameof(Index), TempData["user"] = _user.UserName);
                    var vM = new GetApartmentsViewModel
                    {
                        Apartments = await _data.GetApartmentsAsync(_user.Id),
                        Contributions = await _data.GetUserContributionsAsync(_user.Id)
                    };
                    return PartialView("GetApartments", vM);
                }
                _notyf.Error("Something went wrong :( ");
                return PartialView();
            }
            _notyf.Warning("Insert all fields!!");
            return PartialView();
        }

        public async Task<IActionResult> Delete(Apartment apartment)
        {
            var delete = await _data.DeleteApartmentAsync(apartment);
            if (delete == true)
            {
                var ownedApartments = await _data.GetApartmentsAsync(_user.Id);
                _notyf.Success("Apartment deleted successfully!");
                return RedirectToAction(nameof(Index), TempData["user"] = _user.UserName);
            }
            _notyf.Error("Something went wrong :( ");
            return RedirectToAction(nameof(Index), TempData["user"] = _user.UserName);
        }
        [HttpGet]
        public async Task<IActionResult> ApartmentTasks(Apartment apartment)
        {
            var ownedApartments = await _data.GetApartmentsAsync(_user.Id);
            return PartialView(ownedApartments);
        }
        [HttpGet]
        public IActionResult WelcomePage()
        {
            return PartialView();
        }
        [HttpGet]
        public IActionResult Account()
        {
            ViewBag.UserDetails = _user;
            return PartialView();
        }
        public async Task<IActionResult> UpdateUserAccount(string userName, string email, string phoneNumber)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(email))
            {
                var updatePersone = await _data.UpdateUserAsync(_user, userName, email, phoneNumber);
                if (updatePersone != null)
                {
                    _notyf.Success("Data updated!");
                    return RedirectToAction(nameof(Index), TempData["user"] = updatePersone);
                }
            }
            return RedirectToAction(nameof(Index), TempData["user"] = _user.UserName);
        }
        public async Task<IActionResult> RemoveUser(string userName)
        {
            if (!string.IsNullOrEmpty(userName) && userName == _user.UserName)
            {
                await _data.DeleteUserAsync(_user);
                _notyf.Success("Your account has been removed");
                return RedirectToAction(nameof(Index));
            }
            _notyf.Warning("Insert your correct username");
            return RedirectToAction(nameof(Index), TempData["user"] = _user.UserName);
        }
        [HttpPost]
        public async Task<IActionResult> EditApartment(string id, string apartmentName, int kitchen, int toilet, int room)
        {
            var update = await _data.UpdateApartment(id, apartmentName, kitchen, toilet, room);
            var ownedApartments = await _data.GetApartmentsAsync(_user.Id);
            if (update)
            {
                _notyf.Success("Apartment updated!");
                return PartialView("ApartmentTasks", ownedApartments);
            }
            _notyf.Error("Something went wrong, please try later :/");
            return PartialView("ApartmentTasks", ownedApartments);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask(string appartmentId, string task)
        {
            var createTask = await _data.CreateTaskAsync(appartmentId, task);
            var currentAppartment = await _data.GetSingleApartmentAsync(appartmentId);
            if (createTask)
            {
                _notyf.Success("New task!");
                return PartialView("TaskList", currentAppartment.Jobs);
            }
            _notyf.Error("Something went wrong");
            return PartialView("TaskList", currentAppartment);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var idApart = await _data.GetApartmentByJobAsync(id);
            var deleteTask = await _data.DeleteTaskAsync(id);
            if (deleteTask)
            {
                _notyf.Success("Removed!");
            }
            else
            {
                _notyf.Error("Something went wrong!");
            }
            var currentAppartment = await _data.GetSingleApartmentAsync(idApart);

            return PartialView("TaskList", currentAppartment.Jobs);
        }
        [HttpPost]
        public async Task<IActionResult> AddContributor(string subId, string apartmentId)
        {
            var vM = new GetApartmentsViewModel
            {
                Apartments = await _data.GetApartmentsAsync(_user.Id),
                Contributions = await _data.GetUserContributionsAsync(_user.Id)
            };
            if (subId==_user.Id)
            {
                _notyf.Warning("You can't add yourself as contributor!");
                return PartialView("ApartmentTasks", vM);
            }
            var result = await _data.AddContributorsAsync(subId, apartmentId);
            if (result.Equals("Successfull!"))
            {
                _notyf.Success(result);
                return PartialView("ApartmentTasks", vM);
            }
            _notyf.Error(result);
            return PartialView("ApartmentTasks", vM);

        }
        [HttpPost]
        public async Task<IActionResult>RemoveContributor(string subId,string apartmentId)
        {
 
            var result =await _data.RemoveUserContributionAsync(subId, apartmentId);
            if(result)
            {
                _notyf.Success("Contributor removed!");
                return RedirectToAction(nameof(Index), TempData["user"] = _user.UserName);
            }
            _notyf.Warning("Something went wrong");
            return RedirectToAction(nameof(Index), TempData["user"] = _user.UserName);
        }

    }
}
