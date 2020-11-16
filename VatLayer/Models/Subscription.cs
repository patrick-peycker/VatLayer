using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace VatLayer.Models
{
	public class Subscription
	{
		[Required]
		[Display(Name = "Email address")]
		public string Email { get; set; }

		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Display(Name = "Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		// Select Component

		[Display(Name = "Role")]
		public string RoleSelected { get; set; }

		public List<SelectListItem> Roles { get; }
			= Role.Roles
					.Select(role => new SelectListItem { Value = role, Text = role })
					.ToList();
	}
}
