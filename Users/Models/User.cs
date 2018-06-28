using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int Age { get; set; }
        public string Street { get; set; }
        public int AddressID { get; set; }
        public string Country { get; set; }
        public int CountryID { get; set; }
        public int Phone { get; set; }
        public string PhoneType { get; set; }
        public int PhoneTypeID { get; set; }
    }
}
