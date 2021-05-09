using ApartmentScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Interfaces
{
    public interface IDataService
    {
        Task<string> RegisterAsync(string email, string nick, string password);
    }
}
