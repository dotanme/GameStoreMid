using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreMid.Models
{
    public class ProductOrder
    {
        [Required]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        //how many products from the same type in the order
        public int? Quantity { get; set; }
    }
}
