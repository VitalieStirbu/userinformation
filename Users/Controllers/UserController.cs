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
        public const string CONNECTION_STRING = @"Server=(LocalDb)\MSSQLLocalDB;Database=UserInformation;Trusted_Connection=True";

        DbHandler _dbHandler = new DbHandler();


        public bool HasMorePhones(List<User> users)
        {
            int numberOfPhones = 0;

            foreach (var user in users)
            {
                if (user.Phone != 0)
                    numberOfPhones++;
            }

            return numberOfPhones != 1;
        }

        public IActionResult Index()
        {
            List<User> users = new List<User>();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
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
                                Country = reader["Country"].ToString(),
                                Street = reader["Street"].ToString(),
                                Phone = Convert.ToInt32(reader["Phone"]),
                                PhoneType = reader["PhoneType"].ToString()
                            };
                            users.Add(user);
                        }
                    }
                }
            }

            ViewBag.HasMorePhones = HasMorePhones(users);
            return View(users);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            ViewBag.Phones = _dbHandler.ReturnPhoneTypeList();
            ViewBag.Countries = _dbHandler.ReturnCountryList();
            ViewBag.Addresses = _dbHandler.ReturnAddressesList();

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
            ViewBag.Phones = _dbHandler.ReturnPhoneTypeList();
            ViewBag.Countries = _dbHandler.ReturnCountryList();
            ViewBag.Addresses = _dbHandler.ReturnAddressesList();

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
            ViewBag.Phone = _dbHandler.ReturnPhoneTypeList();

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
            ViewBag.Country = _dbHandler.ReturnCountryList();
            ViewBag.Addresses = _dbHandler.ReturnAddressesList();

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
                com.Parameters.AddWithValue("@AddressID", user.AddressID);
                com.Parameters.AddWithValue("@CountryID", user.CountryID);
                com.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAddress(int streetID, int countryID)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_DeleteAddress", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@AddressID", streetID);
                com.Parameters.AddWithValue("@CountryID", countryID);
                com.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}