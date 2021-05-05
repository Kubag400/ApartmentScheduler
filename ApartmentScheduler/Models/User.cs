using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApartmentScheduler.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Nick { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Apartment> Apartments { get; set; }
        public bool isAdmin { get; set; }
    }
}
