using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreMid.Models
{
    public abstract class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Expected Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime ExpectedDate { get; set; }

        [Display(Name = "Product Orders")]
        [Required]
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }

        public double Total { get; set; }
    }
}
