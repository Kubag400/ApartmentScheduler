using ApartmentScheduler.Data;
using ApartmentScheduler.Interfaces;
using ApartmentScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Services
{
    class DataService : IDataService
    {
        private readonly DataContext _dataContext;
        public DataService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> RegisterAsync(User user)
        {
           await _dataContext.Users.AddAsync(user);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }
    }
}
