using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Users.Models;

namespace Users
{
    public class DbHandler
    {
        public const string CONNECTION_STRING = @"Server=(LocalDb)\MSSQLLocalDB;Database=UserInformation;Trusted_Connection=True";

        public List<Country> ReturnCountryList()
        {
            var countries = new List<Country>();
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("select * from Countries", con))
            {
                con.Open();
                com.CommandType = System.Data.CommandType.Text;

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
            return countries;
        }

        public List<PhoneType> ReturnPhoneTypeList()
        {
            List<PhoneType> phoneTypes = new List<PhoneType>();
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
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

            return phoneTypes;
        }

        public List<Addresses> ReturnAddressesList()
        {
            List<Addresses> addresses = new List<Addresses>();
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



        public List<User> GetAllUsers()
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
                                PhoneID = Convert.ToInt32(reader["PhoneID"]),
                                PhoneType = reader["PhoneType"].ToString()
                            };
                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        public void AddUser(User user)
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
        }

        public void DeleteUser(int id)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_DeleteUser", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", id);
                com.ExecuteNonQuery();
            }
        }

        public void EditUser(User user)
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
                com.Parameters.AddWithValue("@OldAddressID", user.OldAddressID);
                com.Parameters.AddWithValue("@OldCountryID", user.OldCountryID);
                com.Parameters.AddWithValue("@PhoneID", user.PhoneID);
                com.Parameters.AddWithValue("@PhoneTypeID", user.PhoneTypeID);
                com.ExecuteNonQuery();
            }
        }

        public User CurrentUserInfo(int id, string oldAddress, string oldCountry)
        {
            var user = new User();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_CurrentUserInfo", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@ID", id);
                com.Parameters.AddWithValue("@OldAddress", oldAddress);
                com.Parameters.AddWithValue("@OldCountry", oldCountry);

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            FirstName = reader["FirstName"].ToString(),
                            SecondName = reader["SecondName"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Street = reader["Street"].ToString(),
                            Country = reader["Country"].ToString(),
                            Phone = Convert.ToInt32(reader["Phone"]),
                            PhoneType = reader["PhoneType"].ToString(),
                            OldAddressID = Convert.ToInt32(reader["AddressID"]),
                            OldCountryID = Convert.ToInt32(reader["CountryID"])
                        };
                    }
                }
                com.ExecuteNonQuery();
            }

            return user;
        }

        public void AddPhone(User user)
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
        }

        public void DeletePhone(int phoneNumber)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_DeletePhone", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                com.ExecuteNonQuery();
            }
        }

        public void AddAddress(User user)
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
        }

        public void DeleteAddress(int userID, string street)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_DeleteAddress", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", userID);
                com.Parameters.AddWithValue("@Street", street);
                com.ExecuteNonQuery();
            }
        }

        public void DeleteCountry(string country, int userID)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand com = new SqlCommand("sp_DeleteCountry", con))
            {
                con.Open();
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", userID);
                com.Parameters.AddWithValue("@Country", country);
                com.ExecuteNonQuery();
            }
        }
    }
}
