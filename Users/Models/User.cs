using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Models
{
    public class User
    {
        public int ID { get; set; }
        [StringLength(60, MinimumLength = 3, ErrorMessage = "NU fi prost!")]
        [Required(ErrorMessage = "FirstName e obligatoriu")]
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int Age { get; set; }
        public string Street { get; set; }
        public int AddressID { get; set; }
        public string Country { get; set; }
        public int CountryID { get; set; }
        public int Phone { get; set; }
        public int PhoneID { get; set; } //phone id from dbo.Phones SQL
        public string PhoneType { get; set; }
        public int PhoneTypeID { get; set; }

        public int OldAddressID { get; set; }
        public int OldCountryID { get; set; }
    }
}
