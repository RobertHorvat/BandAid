using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BandAid.Models;
using Microsoft.AspNetCore.Mvc;

namespace BandAid.Controllers
{
	public class EventController : Controller
	{
		masterContext _database = new masterContext();
		List<Event> _events = new List<Event>();


		[HttpGet]
		public IActionResult AllEvents(int userId)
		{
			foreach (Event e in _database.Event.Where(it => it.User.UserId == userId))
			{
				_events.Add(e);
			}
			ViewBag.Events = _events;
			return View();
		}

		[HttpGet]
		public IActionResult EditEvent(int eventId)
		{
			Event _event = new Event();
			foreach (Event e in _database.Event)
			{
				if (e.EventId == eventId)
					_event = e;
			}
			return View(_event);
		}

		[HttpPut]
		public IActionResult EditEvent(Event ev)
		{
			_database.Event.Update(ev);
			return View();
		}

		[HttpGet]
		public IActionResult NewEvent(int userId)
		{
			Event _event = new Event();
			User _user = _database.User.First(it => it.UserId == userId);
			_event.UserId = userId;
			if (_user.Adress != null)
			{
				_event.Adress = _user.Adress;
			}
			if (_user.PhoneNumber != null)
			{
				_event.PhoneNumber = _user.PhoneNumber;
			}
			return View(_event);
		}

		[HttpPost]
		public IActionResult NewEvent(Event ev)
		{
			if (_database.Event.Last() == null)
			{
				ev.EventId = 1;
			}
			else
				ev.EventId = _database.Event.Last().EventId + 1;

			return View();
		}
	}
}