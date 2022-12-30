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
    public class CreateSession : ControllerBase
    {
        protected MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public CreateSession(IConfiguration configuration, IWebHostEnvironment env, IMemoryCache cache)
        {
            _configuration = configuration;
            _env = env;
            string connStr = ConfigurationExtensions.GetConnectionString(_configuration, "ConnectionStringForDatabase");
            con = new MySqlConnection(connStr);
        }


        [HttpGet]
        [Route("{username}/{password}")]
        //[Route("set")]
        public JsonResult Set(string username, string password)
        {
            var user = GetUser(username, password);

            if (user != null)
            {
                HttpContext.Session.Set<User>(username, user);
                return new JsonResult($"{user.user_name}, save to session");
            }
            else
            {
                return new JsonResult("User not found");
            }
        }

        [HttpGet]
        [Route("get")]
        public JsonResult Get()
        {
            User user = HttpContext.Session.Get<User>("Nizam");
            return new JsonResult($"{user.user_name}, info fetched from session");
        }

        [HttpGet]
        [Route("clear")]
        public JsonResult ClearSession()
        {
            HttpContext.Session.Clear();
            return new JsonResult($"session clear");
        }

        protected User GetUser(string username, string password)
        {
            User user = null;
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from user where user_name ='" + username +"'", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        user_nr = (SByte)reader["user_id"],
                        user_name = reader["user_name"].ToString(),
                        user_password = reader["user_password"].ToString(),
                    };

                    if (BCrypt.Net.BCrypt.Verify(password, user.user_password))
                        return user;
                    return null;
                } 
                else
                {
                    Debug.WriteLine("User not found");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Get User failed with Ex: " + ex.ToString());
            }
            con.Close();
            return user;
        }

        protected void InsertNewUser(User user)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Insert failed with Ex: " + ex);
            }
        }
    }
}