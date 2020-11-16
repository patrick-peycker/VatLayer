using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VatLayer.Models
{
	public class User : IdentityUser<int>
	{
		[PersonalData]
		[Display(Name = "Fist Name")]
		public string FirstName { get; set; }

		[PersonalData]
		[Display(Name = "Last Name")]

		public string LastName { get; set; }
		[PersonalData]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
		[Display(Name = "Date of Birth")]
		
		public DateTime DateOfBirth { get; set; }
	}
}
