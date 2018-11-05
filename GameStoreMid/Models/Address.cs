using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreMid.Models
{
    public class Address
    {
        public int AddressID { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int? ZipCode { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } 
    }
}
