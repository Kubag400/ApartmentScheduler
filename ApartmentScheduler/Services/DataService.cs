﻿using ApartmentScheduler.Data;
using ApartmentScheduler.Interfaces;
using ApartmentScheduler.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Apartment>> GetOwnedApartmentsAsync(string nick)
        {
            var user = await _userManager.FindByNameAsync(nick);
            return  await _dataContext.Apartments.Where(x => x.Owner == user).ToListAsync();
        }

        public async Task<IdentityUser> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if(!userHasValidPassword)
            {
                return null;
            }
            return user;
        }

        public async Task<string> RegisterAsync(string email, string nick, string password)
        {
            var checkEmail = await _userManager.FindByEmailAsync(email);
            var checkNick = await _userManager.FindByNameAsync(nick);
            if (checkEmail != null && checkNick !=null)
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
