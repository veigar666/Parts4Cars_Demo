using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parts4Cars_Demo.Models
{
    public class Part
    {
        public Part()
        {
            this.PartsInUserCart = new HashSet<ApplicationUser>();
            this.Orders = new HashSet<Order>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string PartMake { get; set; }

        [StringLength(50)]
        public string CarMake { get; set; }

        [StringLength(50)]
        public string CarModel { get; set; }
        public int CarYear { get; set; }

        [StringLength(50)]
        public string Category { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<ApplicationUser> PartsInUserCart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
