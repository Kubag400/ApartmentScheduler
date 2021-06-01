using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApartmentScheduler.Models
{
    public class Contributor
    {
        [Key]
        public int Id { get; set; }
        public string ContributorName { get; set; }
        public string ContributorId { get; set; }
        public virtual Apartment Apartment { get; set; }

    }
}