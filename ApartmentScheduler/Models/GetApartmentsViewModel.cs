using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Models
{
    public class GetApartmentsViewModel
    {
        public List<Apartment> Apartments{ get; set; }
        public List<Apartment> Contributions { get; set; }
    }
}
