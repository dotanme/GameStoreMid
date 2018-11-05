using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreMid.Models
{
    public class ProductTag
    {
        [Required]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        public int TagID { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
