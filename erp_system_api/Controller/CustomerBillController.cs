using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using erp_system_api;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace erp_system_api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerBillController : ControllerBase
    {
        protected MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public CustomerBillController(IConfiguration configuration, IWebHostEnvironment env, IMemoryCache cache)
        {
            _configuration = configuration;
            _env = env;
            string connStr = ConfigurationExtensions.GetConnectionString(_configuration, "ConnectionStringForDatabase");
            con = new MySqlConnection(connStr);
        }

        [HttpGet]
        public JsonResult Get()
        {
            List<CustomerBill> customerBillList = new List<CustomerBill>();
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from customerBill", con);
                Int32 count = (Int32)cmd.ExecuteScalar();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < count; i++)
                    {
                        CustomerBill customerBill = new CustomerBill
                        {
                            CustomerBillNr = (Int32)reader["CustomerBillNr"],
                            CompanyName = reader["CompanyName"].ToString(),
                            ContactPerson = reader["ContactPerson"].ToString(),
                            CustomerStreet = reader["CustomerStreet"].ToString(),
                            CustomerPostcode = reader["CustomerPostcode"].ToString(),
                            Amount = (decimal)reader["Amount"],
                            Currency = reader["Currency"].ToString(),
                            IssuedOn = (DateTime)reader["IssuedOn"],
                            PaymentDate = (DateTime)reader["PaymentDate"],
                        };
                        customerBillList.Add(customerBill);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult(customerBillList);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            CustomerBill customerBill = null;
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from customerBill where CustomerBillNr =" + id, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customerBill = new CustomerBill
                    {
                        CustomerBillNr = (Int32)reader["CustomerBillNr"],
                        CompanyName = reader["CompanyName"].ToString(),
                        ContactPerson = reader["ContactPerson"].ToString(),
                        CustomerStreet = reader["CustomerStreet"].ToString(),
                        CustomerPostcode = reader["CustomerPostcode"].ToString(),
                        Amount = (decimal)reader["Amount"],
                        Currency = reader["Currency"].ToString(),
                        IssuedOn = (DateTime)reader["IssuedOn"],
                        PaymentDate = (DateTime)reader["PaymentDate"],
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult(customerBill);
        }

        [HttpPost]
        public JsonResult Post(CustomerBill bill)
        {
            try
            {
                con.Open();
                var cmd = new MySqlCommand("INSERT INTO customerbill (CompanyName,ContactPerson,CustomerStreet,CustomerPostcode, Amount,Currency,IssuedOn,PaymentDate) VALUES (@CompanyName,@ContactPerson,@CustomerStreet,@CustomerPostcode,@Amount,@Currency,@IssuedOn,@PaymentDate)", con);
                cmd.Parameters.AddWithValue("@CompanyName", bill.CompanyName);
                cmd.Parameters.AddWithValue("@ContactPerson", bill.ContactPerson);
                cmd.Parameters.AddWithValue("@CustomerStreet", bill.CustomerStreet);
                cmd.Parameters.AddWithValue("@CustomerPostcode", bill.CustomerPostcode);
                cmd.Parameters.AddWithValue("@Amount", bill.Amount);
                cmd.Parameters.AddWithValue("@Currency", bill.Currency);

                DateTime io = bill.IssuedOn;
                Debug.WriteLine("Date 1 " + bill.IssuedOn);
                io.ToString("dd/MM/yyyy");
                cmd.Parameters.AddWithValue("@IssuedOn", io);

                DateTime pd = bill.PaymentDate;
                pd.ToString("dd/MM/yyyy");
                cmd.Parameters.AddWithValue("@PaymentDate", pd);
                
                var result = cmd.ExecuteNonQuery();
                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult("Done");
        }

        [HttpPut]
        public JsonResult Put(CustomerBill bill)
        {
            try
            {
                con.Open();
                var cmd = new MySqlCommand("Update customerBill set CompanyName=@CompanyName, ContactPerson=@ContactPerson, CustomerStreet=@CustomerStreet, CustomerPostcode=@CustomerPostcode, Amount=@Amount, Currency=@Currency, IssuedOn=@IssuedOn, PaymentDate=@PaymentDate where customerBillNr =" + bill.CustomerBillNr, con);
                cmd.Parameters.AddWithValue("@CompanyName", bill.CompanyName);
                cmd.Parameters.AddWithValue("@ContactPerson", bill.ContactPerson);
                cmd.Parameters.AddWithValue("@CustomerStreet", bill.CustomerStreet);
                cmd.Parameters.AddWithValue("@CustomerPostcode", bill.CustomerPostcode);
                cmd.Parameters.AddWithValue("@Amount", bill.Amount);
                
                DateTime io = bill.IssuedOn;
                Debug.WriteLine("Date 1 " + bill.IssuedOn);
                io.ToString("dd/MM/yyyy");
                cmd.Parameters.AddWithValue("@IssuedOn", io);

                DateTime pd = bill.PaymentDate;
                pd.ToString("dd/MM/yyyy");
                cmd.Parameters.AddWithValue("@PaymentDate", pd);

                var result = cmd.ExecuteNonQuery();
                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult("Done");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                con.Open();
                var cmd = new MySqlCommand("DELETE FROM customerbill WHERE CustomerBillNr =" + id, con);
                var result = cmd.ExecuteNonQuery();
                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult("Done");
        }
    }
    
}
