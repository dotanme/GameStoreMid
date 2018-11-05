using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreMid.Models
{
    public class BrowsingHistory
    {
        [Key]
        public int BrowsingHistroyID { get; set; }

        public string UserName { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int ProductID { get; set; }

        public virtual Product Product { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Viewed { get; set; }


    }
}
