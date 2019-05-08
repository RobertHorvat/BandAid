using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BandAid.Models;
using BandAid.Models.PomocneKlase;
using Microsoft.AspNetCore.Mvc;

namespace BandAid.Controllers
{
    public class UserController : Controller
    {
        masterContext _database = new masterContext();
        List<UserRole> _roles = new List<UserRole>();


        //Registration 
        [HttpGet]
        public IActionResult Registration()
        {
            foreach (UserRole r in _database.UserRole.ToList())
            {
                _roles.Add(r);
            }
            ViewBag.Roles = _roles;
            return View();
        }

        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(User user)
        {
            bool _status = false;
            string _message = "";
            if (ModelState.IsValid)
            {
                if (EmailExist(user.Email))
                {
                    ModelState.AddModelError("Email vec u upotrebi", "Ovaj Email se koristi,odaberite drugi");
                    return View(user);
                }

                user.ActivationCode = Guid.NewGuid();
                user.PassHash = Enkripcija.Hash(user.PassHash);

                if (_database.User.Last() == null)
                {
                    user.UserId = 1;
                }
                else
                    user.UserId = _database.User.Last().UserId + 1;


                user.IsEmailVerified = false;

                _database.User.Add(user);
                //Obavezno
                _database.SaveChanges();


                //TODO:send email method
                _message = "Uspješno ste se registrirali. Link za aktivaciju" +
                    "Vam je poslan na Vašu email adresu: " + user.Email;
                _status = true;
            }
            else
                _message = "Invalid Request";

            ViewBag.Message = _message;
            ViewBag.Status = _status;
            return View(user);
        }

        [NonAction]
        public bool EmailExist(string email)
        {

            var exists = _database.User.Where(a => a.Email == email).FirstOrDefault();
            return exists != null;

        }
        //Verify Email
        //TODO
        //Verify Email Link
        //TODO
        //Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //Login POST action
        //TODO

        //Logout
    }
}