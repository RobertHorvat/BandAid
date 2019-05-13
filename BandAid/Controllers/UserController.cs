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
            .Where(it => it.Name != "Admin")
            .ToList();


        //Registration 
        [HttpGet]
        public IActionResult Registration()
        {
            
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
            if(ModelState.IsValid)
            {
                if(EmailExist(user.Email))
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


                    RegUser.UserId = _database.User.Last().UserId + 1;

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
                    return RedirectToAction("Registration");
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
            return View();
        }

        //Login POST action
        //TODO

        //Logout
    }
}