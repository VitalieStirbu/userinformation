using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Users.Models;

namespace Users.Controllers
{
    public class UserController : Controller
    {
        // De exemplu tu faci niste schimbari, comentariu care il vezi tot se socoate schimbari
        // Noi am scris un comentariu, deci putem s facem push pe git, fix ca dupa instructie
        // test test test
        public const string CONNECTION_STRING = @"Server=(LocalDb)\MSSQLLocalDB;Database=UserInformation;Trusted_Connection=True";

        List<PhoneType> phoneTypes = new List<PhoneType>();
        List<Country> countries = new List<Country>();
        List<Addresses> addresses = new List<Addresses>();


        public List<Country> ReturnCountryList()
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand com = new SqlCommand("select * from Countries", con))
                {
                    con.Open();
                    com.CommandType = CommandType.Text;

                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Country country = new Country()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                CountryType = reader["Country"].ToString()
                            };

                            countries.Add(country);
                        }
                    }
                }
            }
            return countries;
        }

        public List<PhoneType> ReturnPhoneTypeList()
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand com = new SqlCommand("sp_GetPhoneType", con))
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = com.ExecuteReader())
                        while (reader.Read())
                        {
                            PhoneType phoneType = new PhoneType()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Type = reader["PhoneType"].ToString()
                            };
                            phoneTypes.Add(phoneType);
                        }
                }
            }

            return phoneTypes;
        }

        public List<Addresses> ReturnAddressesList()
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("select * from Addresses", con))
            {
                con.Open();
                com.CommandType = CommandType.Text;
                using (SqlDataReader reader = com.ExecuteReader())
                    while (reader.Read())
                    {
                        Addresses address = new Addresses()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Street = reader["Street"].ToString()
                        };
                        addresses.Add(address);
                    }
            }
            return addresses;
        }

        public bool HasMorePhones(List<User> users)
        {
            int numberOfPhones = 0;
            foreach (var user in users)
            {
                if (user.Phone != 0)
                    numberOfPhones++;
            }

            if (numberOfPhones == 1)
                return false;
            else
                return true;
        }


        public IActionResult Index()
        {
            List<User> users = new List<User>();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_GetUsers", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            FirstName = reader["FirstName"].ToString(),
                            SecondName = reader["SecondName"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            //CountryID = Convert.ToInt32(reader["CountryID"]),
                            Country = reader["Country"].ToString(),
                            Phone = Convert.ToInt32(reader["Phone"]),
                            PhoneType = reader["PhoneType"].ToString(),
                            Street = reader["Street"].ToString()
                        };
                        users.Add(user);
                    }
                }
            }

            //ViewBag.HasMorePhones = HasMorePhones(users);
            return View(users);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            ViewBag.Phones = ReturnPhoneTypeList();
            ViewBag.Countries = ReturnCountryList();
            ViewBag.Addresses = ReturnAddressesList();

            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_AddUserInfo", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@FirstName", user.FirstName);
                com.Parameters.AddWithValue("@SecondName", user.SecondName);
                com.Parameters.AddWithValue("@Age", user.Age);
                com.Parameters.AddWithValue("@AddressID", user.AddressID);
                com.Parameters.AddWithValue("@CountryID", user.CountryID);
                com.Parameters.AddWithValue("@Phone", user.Phone);
                com.Parameters.AddWithValue("@PhoneTypeID", user.PhoneTypeID);
                com.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_DeleteUser", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", id);
                com.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            ViewBag.Phones = ReturnPhoneTypeList();
            ViewBag.Countries = ReturnCountryList();
            ViewBag.Addresses = ReturnAddressesList();

            return View();
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_EditUser", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", user.ID);
                com.Parameters.AddWithValue("@FirstName", user.FirstName);
                com.Parameters.AddWithValue("@SecondName", user.SecondName);
                com.Parameters.AddWithValue("@Age", user.Age);
                com.Parameters.AddWithValue("@AddressID", user.AddressID);
                com.Parameters.AddWithValue("@CountryID", user.CountryID);
                com.Parameters.AddWithValue("@Phone", user.Phone);
                com.Parameters.AddWithValue("@PhoneTypeID", user.PhoneTypeID);
                com.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddPhone(int id)
        {
            ViewBag.Phone = ReturnPhoneTypeList();
            return View();
        }

        [HttpPost]
        public IActionResult AddPhone(User user)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_AddPhone", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", user.ID);
                com.Parameters.AddWithValue("@Phone", user.Phone);
                com.Parameters.AddWithValue("@PhoneTypeID", user.PhoneTypeID);
                com.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeletePhone(int phoneNumber)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_DeletePhone", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                com.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddAddress(int id)
        {
            ViewBag.Country = ReturnCountryList();
            ViewBag.Addresses = ReturnAddressesList();

            return View();
        }

        [HttpPost]
        public IActionResult AddAddress(User user)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_AddStreet", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@ID", user.ID);
                com.Parameters.AddWithValue("@AddresID", user.AddressID);
                com.Parameters.AddWithValue("@CountryID", user.CountryID);
                com.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAddress(string streetID)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_DeleteAddress", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@StreetID", streetID);
                com.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}