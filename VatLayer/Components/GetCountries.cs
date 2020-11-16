using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VatLayer.Components
{
	public class GetCountries : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			var countries = new Dictionary<string, string>();
			foreach (var country in VatLayer.Controllers.VatController.rateList.Rates)
			{
				countries.Add(country.Key, country.Value.Country_name);				
			}
			return View(countries);
		}
	}
}
