using ApartmentScheduler.Data;
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
            return await _dataContext.Apartments.Where(x => x.Owner == user).ToListAsync();
        }

        public async Task<IdentityUser> GetUserAsync(string nick)
        {
            return await _userManager.FindByNameAsync(nick);
        }

        public async Task<IdentityUser> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!userHasValidPassword)
            {
                return null;
            }
            return user;
        }

        public async Task<string> RegisterAsync(string email, string nick, string password)
        {
            var checkEmail = await _userManager.FindByEmailAsync(email);
            var checkNick = await _userManager.FindByNameAsync(nick);
            if (checkEmail != null || checkNick != null)
            {
                return "exists";
            }
            var newUser = new IdentityUser
            {
                Email = email,
                UserName = nick,
            };
            var createdUser = await _userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded)
            {
                return createdUser.Errors.Select(x => x.Description).ToString();
            }
            return "Success";


        }
        public async Task<List<Apartment>> GetApartmentsAsync(string ownerId)
        {
            var ownedApartments = await _dataContext.Apartments
                .Include(y => y.Jobs)
                .Include(x => x.Owner)
                .Include(z => z.Contributors)
                .Where(x => x.Owner.Id == ownerId).ToListAsync();
            return ownedApartments;
        }

        public async Task<bool> CreateApartmentAsync(Apartment apartment, string owner)
        {
            var apartmentOwner = await _userManager.FindByNameAsync(owner);
            apartment.Owner = apartmentOwner;
            await _dataContext.Apartments.AddAsync(apartment);
            var added = await _dataContext.SaveChangesAsync();
            return added > 0;
        }

        public async Task<bool> DeleteApartmentAsync(Apartment apartment)
        {
            _dataContext.Apartments.Remove(apartment);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task DeleteUserAsync(IdentityUser user)
        {
            var apartmentsToDelete = await _dataContext.Apartments.Where(x => x.Owner == user).ToListAsync();
            foreach (var apartment in apartmentsToDelete)
            {
                _dataContext.Remove(apartment);
            }
            await _dataContext.SaveChangesAsync();
            await _userManager.DeleteAsync(user);
        }

        public async Task<string> UpdateUserAsync(IdentityUser user, string userName, string email, string phoneNumber)
        {
            var userToUpdate = await _userManager.FindByIdAsync(user.Id);
            userToUpdate.UserName = userName;
            userToUpdate.Email = email;
            userToUpdate.PhoneNumber = phoneNumber;
            var updated = await _userManager.UpdateAsync(userToUpdate);
            if (updated.Succeeded)
            {
                return userToUpdate.UserName;
            }
            return null;
        }

        public async Task<bool> UpdateApartment(string id, string apartmentName, int kitchen, int toilet, int room)
        {
            var newId = Guid.Parse(id);
            var apartmentToUpdate = await _dataContext.Apartments.Where(x => x.Id == newId).FirstAsync();
            apartmentToUpdate.Name = apartmentName;
            apartmentToUpdate.Kitchen = kitchen;
            apartmentToUpdate.Toilet = toilet;
            apartmentToUpdate.Room = room;
            _dataContext.Apartments.Update(apartmentToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> CreateTaskAsync(string id, string task)
        {

            var apartmentForTask = await GetSingleApartmentAsync(id);

            var job = new Job
            {

                Task = task,
                IsDone = false,
            };
            //apartmentForTask.Jobs = new List<Job>();
            apartmentForTask.Jobs.Add(job);
            _dataContext.Apartments.Update(apartmentForTask);
            var newJob = await _dataContext.SaveChangesAsync();
            return newJob > 0;
        }

        public async Task<Apartment> GetSingleApartmentAsync(string id)
        {
            var newId = Guid.Parse(id);
            return await _dataContext.Apartments.Include(x => x.Jobs).Where(y => y.Id == newId).FirstAsync();
        }

        public async Task<string> GetApartmentByJobAsync(string jobId)
        {
            var newId = Guid.Parse(jobId);
            var job = await _dataContext.Jobs.Include(a => a.Apartment).Where(x => x.Id == newId).FirstAsync();
            return job.Apartment.Id.ToString();
        }

        public async Task<bool> DeleteTaskAsync(string id)
        {
            var newId = Guid.Parse(id);
            var jobToDelete = await _dataContext.Jobs.Where(x => x.Id == newId).FirstAsync();
            _dataContext.Jobs.Remove(jobToDelete);
            var jobDeleted = await _dataContext.SaveChangesAsync();
            return jobDeleted > 0;
        }

        public async Task<string> AddContributorsAsync(string id, string apartmentId)
        {
            var userToAdd = await _userManager.FindByIdAsync(id);
            if (userToAdd == null)
            {
                return "Wrong Id!";
            }
            var newApartmentId = Guid.Parse(apartmentId);
            var apartment = await _dataContext.Apartments.Where(x => x.Id == newApartmentId).FirstAsync();
            var newContributor = new Contributor
            {
                ContributorId = userToAdd.Id,
                ContributorName = userToAdd.UserName
            };
            apartment.Contributors.Add(newContributor);
            var subAdded = await _dataContext.SaveChangesAsync();
            if (subAdded > 0)
            {
                return "Successfull!";
            }
            return "Method issue";
        }

        public async Task<List<Apartment>> GetUserContributionsAsync(string userId)
        {
            var contributions = await _dataContext.Contributors
                .Include(x => x.Apartment)
                .Include(x => x.Apartment.Owner)
                .Where(x => x.ContributorId == userId)
                .ToListAsync();
            var apartmentList = new List<Apartment>();
            foreach (var item in contributions)
            {
                var appToAdd = await _dataContext.Apartments.Where(x => x.Id == item.Apartment.Id).FirstOrDefaultAsync();
                apartmentList.Add(appToAdd);
            }
            return apartmentList;
        }
    }
}
