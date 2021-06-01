using Microsoft.AspNetCore.Identity;
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
        [Required]
        public string Name { get; set; }
        [Required]
        public int Room { get; set; }
        [Required]
        public int Toilet { get; set; }
        public int Kitchen { get; set; }
        public virtual IdentityUser Owner { get; set; }
        public virtual IEnumerable<IdentityUser> SubUsers { get; set; }
        public virtual List<Job> Jobs { get; set; } = new List<Job>();
    }
}
