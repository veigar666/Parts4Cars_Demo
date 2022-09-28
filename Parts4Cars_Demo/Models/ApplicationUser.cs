using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parts4Cars_Demo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Cart = new HashSet<Part>();
            this.Orders = new HashSet<Order>();
        }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }
        public virtual ICollection<Part> Cart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
