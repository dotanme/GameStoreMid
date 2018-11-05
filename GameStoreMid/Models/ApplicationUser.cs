using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GameStoreMid.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int AddressID { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<BrowsingHistory> BrowsingHistories { get; set; }

        public virtual ICollection<ClientOrder> Orders { get; set; }
    }
}
