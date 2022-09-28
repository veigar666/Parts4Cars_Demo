using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parts4Cars_Demo.Models
{
    public class Order
    {
        public Order()
        {
            this.PartsInOrder = new HashSet<Part>();
        }

        public int Id { get; set; }
        public DateTime OrderedOn { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string SecondName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }
        public decimal Price { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Part> PartsInOrder { get; set; }
    }
}
