
namespace erp_system_api
{
    public class CustomerBill
    {
        public String CompanyName { get; set; }
        public String ContactPerson { get; set; }

        public String CustomerStreet { get; set; }

        public String CustomerPostcode { get; set; }

        public decimal Amount { get; set; }

        public String Currency { get; set; }

        public DateTime IssuedOn { get; set; }

        public DateTime PaymentDate { get; set; }

        public String State { get; set; }

        public Int32 CustomerBillNr { get; set; }
    }
}
