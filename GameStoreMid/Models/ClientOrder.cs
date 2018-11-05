using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreMid.Models
{
    public class ClientOrder : Order
    {
        [ForeignKey("ApplicationUserID")]
        public string ApplicationUserID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
