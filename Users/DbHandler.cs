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
    }
}
