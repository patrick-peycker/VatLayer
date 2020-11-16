using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VatLayer.Models
{
	public class Role : IdentityRole<int>
	{
		public static string[] Roles = new string [] { "Administrator", "Manager", "Guest" };
	}
}
