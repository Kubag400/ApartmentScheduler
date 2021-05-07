using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmentScheduler.Models
{
    public class Apartment
    {
        [Key]
        public Guid  Id { get; set; }
        public string Name { get; set; }
        public int Room { get; set; }
        public int Toilet { get; set; }
        public int Kitchen { get; set; }
        public User Owner { get; set; }
        public List<User> Users { get; set; }

    }
}
