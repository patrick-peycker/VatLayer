using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VatLayer.Components
{
	public class GetRates : ViewComponent
	{
		public IViewComponentResult Invoke(string codeCountry)
		{
			var content = VatLayer.Controllers.VatController.rateList.Rates.FirstOrDefault(x => x.Key == codeCountry);
			var rate = new VatLayer.Json.Rate
			{ 
				Country_name = content.Value.Country_name,
				Standard_rate = content.Value.Standard_rate,
				Reduced_rates = content.Value?.Reduced_rates
			};

			return View(rate);
		}
	}
}
