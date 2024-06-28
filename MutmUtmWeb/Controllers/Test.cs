using Microsoft.AspNetCore.Mvc;
using System;

namespace MutmUtmWeb.Controllers
{
	public class Test : Controller
	{
		public IActionResult Index()
		{
			List<Person> people = GetPeople(); // Replace with your logic to fetch people
			return View(people);
		}

		private List<Person> GetPeople()
		{
			// Replace this with your actual logic to fetch people from database or elsewhere
			return new List<Person>
			{
			new Person { Id = 1, Name = "John" },
			new Person { Id = 2, Name = "Jane" },
			new Person { Id = 3, Name = "Alice" }
		};
		}
	}
	public class Person
	{
		public int Id { get; set; }
		public string Name { get; set; }	
	}
}
