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
    }
}
