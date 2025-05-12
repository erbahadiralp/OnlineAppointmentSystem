using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Entity.Concrete
{
    public class AppUser : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual Employee Employee { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
