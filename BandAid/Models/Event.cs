using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BandAid.Models
{
	public partial class Event
	{
		public int EventId { get; set; }

		[Required(ErrorMessage = "Ime događaja je obavezno")]
		[Display(Name = "Događaj")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Vrijeme događaja je obavezno")]
		[DataType(DataType.DateTime, ErrorMessage = "Uneseno vrijeme nije točno")]
		public DateTime Date { get; set; }

		[Required(ErrorMessage = "Adresa je obavezna")]
		[Display(Name = "Adresa")]
		public string Adress { get; set; }

		[Display(Name = "Kontakt broj")]
		public string PhoneNumber { get; set; }

		[Display(Name = "Opis")]
		public string Description { get; set; }

		[DataType(DataType.ImageUrl)]
		public string ImgUrl { get; set; }
		public int UserId { get; set; }
		public int StatusId { get; set; }

		public User User { get; set; }

		public Status Status { get; set; }
	}
}
