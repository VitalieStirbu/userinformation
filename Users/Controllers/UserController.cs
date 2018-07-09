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
            ViewBag.HasMorePhones = HasMorePhones(_dbHandler.GetAllUsers());
            return View(_dbHandler.GetAllUsers());
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
            _dbHandler.AddUser(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            _dbHandler.DeleteUser(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditUser(int id, int phoneID, string address, string country)
        {
          //var user = _dbHandler.GetUser(id);
            ViewBag.Phones = _dbHandler.ReturnPhoneTypeList();
            ViewBag.Countries = _dbHandler.ReturnCountryList();
            ViewBag.Addresses = _dbHandler.ReturnAddressesList();

            return View(_dbHandler.CurrentUserInfo(id, address, country));
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            //if (ModelState.IsValid)
            //{
            //_dbHandler.EditUser(user);
            //return RedirectToAction("Index");
            //}

            _dbHandler.EditUser(user);

            //ViewBag.Phones = _dbHandler.ReturnPhoneTypeList();
            //ViewBag.Addresses = _dbHandler.ReturnAddressesList();
            //ViewBag.Countries = _dbHandler.ReturnCountryList();

            return RedirectToAction(nameof(Index));
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
            _dbHandler.AddPhone(user);
            return RedirectToAction("Index");
        }

        public IActionResult DeletePhone(int phoneNumber)
        {
            _dbHandler.DeletePhone(phoneNumber);
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
            _dbHandler.AddAddress(user);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAddress(int userID, string street)
        {
            _dbHandler.DeleteAddress(userID, street);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteCountry(string country, int userID)
        {
            _dbHandler.DeleteCountry(country, userID);
            return RedirectToAction(nameof(Index));
        }
    }
}