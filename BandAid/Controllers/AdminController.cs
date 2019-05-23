using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BandAid.Models;
using Microsoft.AspNetCore.Mvc;
using BandAid.Models.PomocneKlase;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BandAid.Controllers
{
    public class AdminController : Controller
    {
        private masterContext _database = new masterContext();



        [HttpGet]
        public IActionResult Index()
        {

            if (HttpContext.Session.GetObjectFromJson<User>("user") == null || HttpContext.Session.GetObjectFromJson<User>("user").RoleId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Event> _events = _database.Event.ToList();
                List<User> _users = _database.User.ToList();
                if (!_events.Any())
                {
                    ViewBag.SviEventi = 0.ToString();
                    ViewBag.DostupniEventi = 0.ToString();
                    ViewBag.ZavrseniEventi = 0.ToString();
                }
                else
                {
                    ViewBag.SviEventi = _events.Count()
                                               .ToString();
                    ViewBag.DostupniEventi = _events.Where(it => it.StatusId == 5)
                                                    .Count()
                                                    .ToString();
                    ViewBag.ZavrseniEventi = _events.Where(it => it.StatusId == 4)
                                                    .Count()
                                                    .ToString();
                    ViewBag.Admini = _users.Where(it => it.RoleId == 1)
                                           .Count()
                                           .ToString();
                    ViewBag.Izvodaci = _users.Where(it => it.RoleId == 2)
                                           .Count()
                                           .ToString();
                    ViewBag.Organizatori = _users.Where(it => it.RoleId == 3)
                                           .Count()
                                           .ToString();
                    ViewBag.Potvrdeni = _users.Where(it => it.IsEmailVerified)
                                           .Count()
                                           .ToString();
                    ViewBag.Neaktivni = _users.Where(it => !it.IsEmailVerified)
                                           .Count()
                                           .ToString();
                }

                User admin = HttpContext.Session.GetObjectFromJson<User>("user");
                return View(admin);
            }


        }
        [HttpGet]
        public IActionResult Users()
        {
            if (HttpContext.Session.GetObjectFromJson<User>("user") == null || HttpContext.Session.GetObjectFromJson<User>("user").RoleId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<User> _users = _database.User.ToList();
                if (!_users.Any())
                {
                    ViewBag.NemaKorisnika = "Nema korisnika u bazi!";
                }
                else
                {
                    ViewBag.Korisnici = _users;

                }
                return View(HttpContext.Session.GetObjectFromJson<User>("user"));
            }

        }
        [HttpPost]
        public IActionResult Users(string uloga, string status, string searchString)
        {
            if (HttpContext.Session.GetObjectFromJson<User>("user") == null || HttpContext.Session.GetObjectFromJson<User>("user").RoleId != 1)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {

                List<User> _users = new List<User>();
                if (uloga != null)
                {
                    switch (uloga)
                    {
                        case ("Admin"):
                            foreach (User r in _database.User.Where(it => it.RoleId == 1))
                            {
                                _users.Add(r);
                            }
                            break;
                        case ("Izvodac"):
                            foreach (User r in _database.User.Where(it => it.RoleId == 2))
                            {
                                _users.Add(r);
                            }
                            break;
                        case ("Organizator"):
                            foreach (User r in _database.User.Where(it => it.RoleId == 3))
                            {
                                _users.Add(r);
                            }
                            break;

                        default:
                            break;
                    }
                }
                else if (status != null)
                {
                    switch (status)
                    {
                        case ("True"):
                            foreach (User r in _database.User.Where(it => it.IsEmailVerified == true))
                            {
                                _users.Add(r);
                            }
                            break;
                        case ("False"):
                            foreach (User r in _database.User.Where(it => it.IsEmailVerified == false))
                            {
                                _users.Add(r);
                            }
                            break;

                        default:
                            break;
                    }

                }
                else if (searchString != null)
                {
                    foreach (User r in _database.User)
                    {
                        if (r.Name.Contains(searchString) || r.Email.Contains(searchString))
                            _users.Add(r);

                    }

                }
                else
                {
                    _users = _database.User.ToList();

                }
                ViewBag.Korisnici = _users;
                ViewBag.SearchString = searchString;
                return View(HttpContext.Session.GetObjectFromJson<User>("user"));
            }

        }
        [HttpGet]
        public IActionResult Events()
        {
            if (HttpContext.Session.GetObjectFromJson<User>("user") == null || HttpContext.Session.GetObjectFromJson<User>("user").RoleId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Event> _events = _database.Event.ToList();
                if (!_events.Any())
                {
                    ViewBag.NemaEvenata = "Nema evenata u bazi!";
                }
                else
                {
                    ViewBag.Eventi = _events;

                }
                return View();
            }
        }
        [HttpGet]
        public IActionResult Settings()
        {
            if (HttpContext.Session.GetObjectFromJson<User>("user") == null || HttpContext.Session.GetObjectFromJson<User>("user").RoleId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User admin = HttpContext.Session.GetObjectFromJson<User>("user");
                return View(admin);
            }
        }
        [HttpPost]
        public JsonResult DeleteUser([FromBody]User _user)
        {
            if (HttpContext.Session.GetObjectFromJson<User>("user") == null || HttpContext.Session.GetObjectFromJson<User>("user").RoleId != 1)
            {
                return Json(null);
            }
            else
            {
                User user = _database.User.First(it => it.Email == _user.Email);
                try
                {
                    _database.User.Remove(user);
                    _database.SaveChanges();
                    return Json(true);
                }
                catch
                {

                    return Json(false);
                }

            }
        }
    }
}