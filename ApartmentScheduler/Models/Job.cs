using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Models
{
    public class Job
    {
        public Guid Id  { get; set; }
        public string Task { get; set; }
        public bool IsDone { get; set; }
        public virtual Apartment Apartment { get; set; }
    }
}
