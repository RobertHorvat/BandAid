using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BandAid.Models;
using BandAid.Models.PomocneKlase;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace BandAid.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
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
            return View();
        }

        public IActionResult About()
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

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
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

            return View(new ContactUser());
        }

        [HttpPost]
        public IActionResult Contact([Bind("FirstName,LastName,Email,Subject,Message")]ContactUser contactUser)
        {
            string _message = "";
            string _info = "INFO";
            ContactUser user = contactUser;
            if(!ModelState.IsValid)
            {
               
                return View();
            }
            else
            {
                try
                {
                    SendMessage(user);
                    _message = "Hvala na pitanju, pokušat ćemo odgovoriti u najkraćem mogućem roku!";
                }
                catch (Exception e)
                {

                    _message = e.Message;
                }
                ViewBag.Message = _message;
                ViewBag.Info = _info;
                return View();
            }

        }

        [NonAction]
        public void SendMessage(ContactUser user)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Question", "band.aid.info2019@gmail.com"));
            message.To.Add(new MailboxAddress("BandAid", "band.aid.info2019@gmail.com"));
            message.Subject = user.Subject;
            message.Body = new TextPart("plain")
            {
                Text = user.Message + " " + user.Email + " " + user.FirstName + " " + user.LastName
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("band.aid.info2019@gmail.com", "bandaid_2019");
                client.Send(message);
                client.Disconnect(true);
            }




        }

       

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
