using ApartmentScheduler.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Interfaces
{
    public interface IDataService
    {
        Task<string> RegisterAsync(string email, string nick, string password);
        Task<IdentityUser> LoginAsync(string email, string password);
        Task <List<Apartment>> GetOwnedApartmentsAsync(string nick);
        Task<IdentityUser> GetUserAsync(string nick);
        Task<List<Apartment>> GetApartmentsAsync(string ownerId);
        Task<bool> CreateApartmentAsync(Apartment apartment,string owner);
        Task<bool> DeleteApartmentAsync(Apartment apartment);
        Task DeleteUserAsync(IdentityUser user);
        Task<string> UpdateUserAsync(IdentityUser user,string userName,string email, string phoneNumber);
        Task<bool> UpdateApartment(string id, string apartmentName, int kitchen, int toilet, int room);
        Task<bool> CreateTaskAsync(string id, string task);
        Task<Apartment> GetSingleApartmentAsync(string id);
        Task<string> GetApartmentByJobAsync(string jobId);
        Task<bool> DeleteTaskAsync(string id);
        Task<string> AddContributorsAsync(string id,string apartmentId);
        Task<List<Apartment>> GetUserContributionsAsync(string userId);
    }
}
