using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreMid.Models
{
    public class Product
    {

        [Key]
        public int ProductID { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Price")]
        public double Cost { get; set; }
        [Required]
        [Display(Name = "Total Quantity")]
        public int TotalQuantity { get; set; }
        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        [Display(Name = "Tags")]
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        [Display(Name = "Deal")]
        public int? DealID { get; set; }

        public virtual Deal Deal { get; set; }
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public virtual ICollection<ProductOrder> ProductOrders { get; set; }

    }

}
