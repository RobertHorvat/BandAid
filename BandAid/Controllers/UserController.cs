using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BandAid.Models;
using BandAid.Models.PomocneKlase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BandAid.Controllers
{
    public class UserController : Controller
    {
        static masterContext _database = new masterContext();
        List<UserRole> _roles = _database.UserRole
            //.Where(it => it.Name != "Admin")
            .ToList();


        //Registration 
        [HttpGet]
        public IActionResult Registration()
        {
            if (HttpContext.Session.GetObjectFromJson<User>("user") != null)
            {
                int role = HttpContext.Session.GetObjectFromJson<User>("user").RoleId;
                switch (role)
                {
                    case 1:
                        return RedirectToAction("Index", "Admin");

                    case 2:
                        return RedirectToAction("Index", "Izvodac");

                    case 3:
                        return RedirectToAction("Index", "Organizator");

                }

            }

            ViewBag.Roles = new SelectList(_roles, "RoleId", "Name");
            return View(new User());
        }

        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration([Bind("Name,Email,PassHash,RoleId")]User user)
        {
            bool _status = false;
            string _message = "";
            ViewBag.Roles = new SelectList(_roles, "RoleId", "Name");

            User RegUser = user;
            if (ModelState.IsValid)
            {
                if (EmailExist(user.Email))
                {
                    ModelState.AddModelError("Email", "Email u upotrebi");
                    return View();
                }

                try
                {
                    UserRole _role = _database.UserRole.First(it => it.RoleId == user.RoleId);
                    RegUser.ActivationCode = Guid.NewGuid();
                    RegUser.PassHash = Enkripcija.Hash(user.PassHash);
                    RegUser.Email = user.Email;

                    if (!_database.User.Any())
                    {
                        RegUser.UserId = 1;
                    }
                    else
                    {
                        RegUser.UserId = _database.User.Last().UserId + 1;
                    }


                    RegUser.Role = _role;
                    RegUser.RoleId = _role.RoleId;
                    RegUser.Name = user.Name;



                    RegUser.IsEmailVerified = false;

                    _database.User.Add(RegUser);
                    //Obavezno
                    _database.SaveChanges();
                    //TODO:send email method
                    _message = "Uspješno ste se registrirali. Link za aktivaciju" +
                        "Vam je poslan na Vašu email adresu: " + user.Email;
                    _status = true;
                    ViewBag.Message = _message;
                    ViewBag.Status = _status;
                    return RedirectToAction("Login");
                }
                catch (Exception)
                {

                    return RedirectToAction("Registration");
                }
            }
            else
            {

                return View();
            }


        }

        [NonAction]
        public bool EmailExist(string email)
        {

            var exists = _database.User.Where(it => it.Email == email).FirstOrDefault();
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
            if (HttpContext.Session.GetObjectFromJson<User>("user") != null)
            {
                int role = HttpContext.Session.GetObjectFromJson<User>("user").RoleId;
                switch (role)
                {
                    case 1:
                        return RedirectToAction("Index", "Admin");
                        
                    case 2:
                        return RedirectToAction("Index", "Izvodac");

                    case 3:
                        return RedirectToAction("Index", "Organizator");
                    
                }
                
            }
            return View(new LogUser());
        }

        //Login POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,PassHash")]LogUser user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {

                if (!EmailExist(user.Email))
                {
                    ModelState.AddModelError("Email", "Nepostojeći email");
                    return View();
                }
                else
                {
                    User logUser = _database.User.First(it => it.Email == user.Email);

                    if (Enkripcija.Hash(user.PassHash) != logUser.PassHash)
                    {
                        ModelState.AddModelError("PassHash", "Kriva zaporka");
                        return View();
                    }
                    else
                    {
                        if (logUser.Role.Name == "Izvodac")
                        {
                            //TODO:begin session as izvodaxc and redirect to izvodac controller
                            HttpContext.Session.SetObjectAsJson("user", logUser);
                            return RedirectToAction("Index", "Izvodac");
                        }
                        else if (logUser.Role.Name == "Organizator")
                        {
                            //TODO:begin session as Organizator and redirect to organizator controller
                            HttpContext.Session.SetObjectAsJson("user", logUser);
                            return RedirectToAction("Index", "Organizator");
                        }
                        else if (logUser.Role.Name == "Admin")
                        {

                            //TODO:begin session as admin and redirect to admin controller
                            HttpContext.Session.SetObjectAsJson("user", logUser);
                            return RedirectToAction("Index","Admin");


                        }
                    }
                    return View();
                }
            }
        }
        //TODO
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index", "Home");
        }

        //Logout
    }
}