using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VatLayer.Json
{
	public class Rate
	{
		[Display(Name = "Code")]
		public string Country_code { get; set; }

		[Display(Name = "Country")]
		public string Country_name { get; set; }

		[Display(Name = "Standard Rate")]
		public int Standard_rate { get; set; }

		public Dictionary<string, decimal> Reduced_rates { get; set; }
	}
}
