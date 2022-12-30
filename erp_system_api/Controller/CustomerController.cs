using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace erp_system_api
{

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        protected MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public CustomerController(IConfiguration configuration, IWebHostEnvironment env, IMemoryCache cache)
        {
            _configuration = configuration;
            _env = env;
            string connStr = ConfigurationExtensions.GetConnectionString(_configuration, "ConnectionStringForDatabase");
            con = new MySqlConnection(connStr);
        }
        
        [HttpGet]
        public JsonResult Get()
        {
          
            List<Customer> customerList = new List<Customer>();
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from customer", con);
                Int32 count = (Int32)cmd.ExecuteScalar();
                MySqlDataReader reader = cmd.ExecuteReader();
                Debug.WriteLine("DB " + con);
                while (reader.Read())
                {
                    for (int i = 0; i < count; i++)
                    {
                       Customer customer = new Customer
                        {
                            CustomerNr = (int)reader["CustomerNr"],
                            CompanyName = reader["CompanyName"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            Lastname = reader["Lastname"].ToString(),
                            Dob = reader["Dob"].ToString(),
                            Street = reader["Street"].ToString(),
                            Nr = reader["Nr"].ToString(),
                            Postcode = reader["Postcode"].ToString(),
                            Country = reader["Country"].ToString(),
                            Cellphone = reader["Cellphone"].ToString(),
                            Landlinephone = reader["Landlinephone"].ToString(),
                            Note = reader["Note"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                        customerList.Add(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
                Debug.WriteLine("Session" + ex.ToString());
            }
            con.Close();
            return new JsonResult(customerList);
        }

        
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            /*
            if (HttpContext.Session.Get("Nizam") is null)
            {
                return new JsonResult(null);
            }
            */
            Customer customer = null;
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from customer where CustomerNr = "+id, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customer = new Customer
                    {
                            CustomerNr = (int)reader["CustomerNr"],
                            CompanyName = reader["CompanyName"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            Lastname = reader["Lastname"].ToString(),
                            Dob = reader["Dob"].ToString(),
                            Street = reader["Street"].ToString(),
                            Nr = reader["Nr"].ToString(),
                            Postcode = reader["Postcode"].ToString(),
                            Country = reader["Country"].ToString(),
                            Cellphone = reader["Cellphone"].ToString(),
                            Landlinephone = reader["Landlinephone"].ToString(),
                            Note = reader["Note"].ToString(),
                            Email = reader["Email"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult(customer);
        }
        
        [HttpPost]
        public JsonResult Post(Customer cus)
        {
            /*
            if (HttpContext.Session.Get("Nizam") is null)
            {
                return new JsonResult(null);
            }
            */
            try
            {
                con.Open();
                var cmd = new MySqlCommand("INSERT INTO customer (CompanyName,Surname,Lastname,Dob,Street,Nr,Postcode,Country,Cellphone,Landlinephone,Note,Email) VALUES (@CompanyName, @Surname,@Lastname,@Dob,@Street,@Nr,@Postcode,@Country,@Cellphone,@Landlinephone,@Note,@Email)", con);
                cmd.Parameters.AddWithValue("@CompanyName", cus.CompanyName);
                cmd.Parameters.AddWithValue("@Surname", cus.Surname);
                cmd.Parameters.AddWithValue("@Lastname", cus.Lastname);
                cmd.Parameters.AddWithValue("@Dob", "2022-02-02");
                cmd.Parameters.AddWithValue("@Street", cus.Street);
                cmd.Parameters.AddWithValue("@Nr", cus.Nr);
                cmd.Parameters.AddWithValue("@Postcode", cus.Postcode);
                cmd.Parameters.AddWithValue("@Country", cus.Country);
                cmd.Parameters.AddWithValue("@Cellphone", cus.Cellphone);
                cmd.Parameters.AddWithValue("@Landlinephone", cus.Landlinephone);
                cmd.Parameters.AddWithValue("@Note", cus.Note);
                cmd.Parameters.AddWithValue("@Email", cus.Email);
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
        public JsonResult Put(Customer cus)
        {
            /*
            if (HttpContext.Session.Get("Nizam") is null)
            {
                return new JsonResult(null);
            }
            */
            try
            {
                con.Open();
                var cmd = new MySqlCommand("Update customer set CompanyName=@CompanyName, Surname=@Surname, Lastname=@Lastname, Dob=@Dob, Street=@Street, Nr=@Nr, Postcode=@Postcode, Country=@Country, Cellphone=@Cellphone, Landlinephone=@Landlinephone, Note=@Note, Email=@Email where CustomerNr ="+cus.CustomerNr, con);
                cmd.Parameters.AddWithValue("@CompanyName", cus.CompanyName);
                cmd.Parameters.AddWithValue("@Surname", cus.Surname);
                cmd.Parameters.AddWithValue("@Lastname", cus.Lastname);
                cmd.Parameters.AddWithValue("@Dob", "2022-02-02");
                cmd.Parameters.AddWithValue("@Street", cus.Street);
                cmd.Parameters.AddWithValue("@Nr", cus.Nr);
                cmd.Parameters.AddWithValue("@Postcode", cus.Postcode);
                cmd.Parameters.AddWithValue("@Country", cus.Country);
                cmd.Parameters.AddWithValue("@Cellphone", cus.Cellphone);
                cmd.Parameters.AddWithValue("@Landlinephone", cus.Landlinephone);
                cmd.Parameters.AddWithValue("@Note", cus.Note);
                cmd.Parameters.AddWithValue("@Email", cus.Email);
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
            /*
            if (HttpContext.Session.Get("Nizam") is null)
            {
                return new JsonResult(null);
            }
            */
            try
            {
                con.Open();
                var cmd = new MySqlCommand("DELETE FROM customer WHERE CustomerNr ="+id, con);
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
