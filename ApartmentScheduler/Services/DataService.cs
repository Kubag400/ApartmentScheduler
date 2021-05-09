using ApartmentScheduler.Data;
using ApartmentScheduler.Interfaces;
using ApartmentScheduler.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Services
{
    class DataService : IDataService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<IdentityUser> _userManager;
        public DataService(DataContext dataContext, UserManager<IdentityUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<string> RegisterAsync(string email, string nick, string password)
        {
            var check = await _userManager.FindByEmailAsync(email);
            if (check != null)
            {
                return "exists";
            }
            var newUser = new IdentityUser
            {
                Email = email,
                UserName = nick,
            };
            var createdUser = await _userManager.CreateAsync(newUser,password);
            if (!createdUser.Succeeded)
            {
                return createdUser.Errors.Select(x => x.Description).ToString();
            }
            return "Success";


        }
    }
}
