using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace GameStoreMid.Models
{
    public class Deal
    {
        [Key]
        public int DealID { get; set; }
        [Range(0, 100)]
        [Required]
        public int PercentageDiscount { get; set; }

        [MinLength(3)]
        [Display(Name = "Deal")]
        public string DescriptionDiscount { get { return Description + " | " + PercentageDiscount + "%"; } }

        public virtual ICollection<Product> Products { get; set; }

        public string Description { get; set; }
    }
}
