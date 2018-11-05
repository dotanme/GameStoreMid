using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreMid.Models
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewID { get; set; }
        [Required]
        [Range(0, 5)]
        [Display(Name = "Stars")]
        public int Rate { get; set; }

        [MaxLength(300), MinLength(6)]
        public string Content { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy HH:mm}",
        ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required]
        public int ProductID { get; set; }

        public virtual Product Product { get; set; }
        [Required]
        public string ApplicationUserID { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get;set;}

    }
}
