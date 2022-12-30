using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_system_api
{
    public class Customer
    {
        public Int32 CustomerNr { get; set; }

        public string CompanyName { get; set; }

        public string Surname { get; set; }

        public string Lastname { get; set; }

        //public DateTime dob { get; set; }
        public string Dob { get; set; }

        public string Street { get; set; }

        public string Nr { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        public string Cellphone { get; set; }

        public string Landlinephone { get; set; }

        public string Note { get; set; }

        public string Email { get; set; }
    }
}
