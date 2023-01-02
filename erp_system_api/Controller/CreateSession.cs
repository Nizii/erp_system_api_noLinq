using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;

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
        [Route("login/{username}/{password}")]
        public JsonResult Login(string username, string password)
        {
            var user = GetUser(username, password);

            if (user != null)
            {
                HttpContext.Session.Set(username, user);
                return new JsonResult($"{user.user_name}, save to session");
            }
            else return new JsonResult("User not found");
        }

        [HttpGet]
        [Route("sign-up/{username}/{password}")]
        public JsonResult SignUp(string username, string password)
        {
            if (CheckIfUserAlreadyExists(username))
            {
                User user = new()
                {
                    user_name = username,
                    user_password = BCrypt.Net.BCrypt.HashPassword(password, 12)
                };

                if (InsertUser(user))
                {
                    HttpContext.Session.Set(username, user);
                    return new JsonResult($"{user.user_name}, save to session");
                }
                return new JsonResult(null);
            }
            else return new JsonResult(null);
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
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from user where user_name ='" + username +"'", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    User user = new User
                    {
                        user_nr = (SByte)reader["user_id"],
                        user_name = reader["user_name"].ToString(),
                        user_password = reader["user_password"].ToString(),
                    };

                    if (BCrypt.Net.BCrypt.Verify(password, user.user_password)) return user;
                    con.Close();
                    return null;
                }
                else
                {
                    con.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Get User failed with Ex: " + ex.ToString());
                con.Close();
                return null;
            }  
        }

        protected Boolean CheckIfUserAlreadyExists(string username)
        {
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from user where user_name ='" + username + "'", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                con.Close();

                if (reader.Read()) return false;
                else return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Insert failed SQL Problem: " + ex.ToString());
            }
            return false;
        }

        protected Boolean InsertUser(User user)
        {
            try
            {
                con.Open();
                var cmd = new MySqlCommand("INSERT INTO user (user_name,user_password) VALUES (@user_name,@user_password)", con);
                cmd.Parameters.AddWithValue("@user_name", user.user_name);
                cmd.Parameters.AddWithValue("@user_password", user.user_password);
                var result = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Insert failed SQL Problem: " + ex.ToString());
            }
            return false;
        }
    }
}