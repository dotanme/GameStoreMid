using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreMid.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        public virtual ICollection<ProductTag>  ProductTags { get; set; }

    }
}
