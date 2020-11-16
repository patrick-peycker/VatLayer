using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VatLayer.Json;

namespace VatLayer.Controllers
{
	public class VatController : Controller
	{
		public static RateList rateList { get; set; }
		public static Rate rate { get; set; }

		private readonly IConfiguration configuration;
		private readonly ILogger logger;

		private const string baseUrl = "http://apilayer.net/api/";

		private string endPoint;
		private string key;
		private string error;

		public VatController(IConfiguration configuration, ILogger<VatController> logger)
		{
			this.configuration = configuration;
			this.logger = logger;
			this.key = configuration["VatLayer:key"];
		}

		public async Task<RateList> GetRateListAsync()
		{
			endPoint = "rate_list";

			using (var client = new HttpClient())
			{
				var httpResponse = await client.GetAsync($"{baseUrl}{endPoint}?access_key={key}");

				if (!httpResponse.IsSuccessStatusCode)
				{
					error = $"Get Rate List - Status Code : {httpResponse.StatusCode}";
					logger.LogError(error);
					throw new Exception(error);
				}

				var content = JsonConvert.DeserializeObject<dynamic>(await httpResponse.Content.ReadAsStringAsync());

				if (!((bool)content.success) || content.rates is null)
				{
					error = $"Get Rate List - {content.error.code} - {content.error.info}";
					logger.LogError(error);
					throw new Exception(error);
				}

				rateList = JsonConvert.DeserializeObject<RateList>(await httpResponse.Content.ReadAsStringAsync());

				return rateList;
			}
		}


		public async Task<Rate> GetRateByCountryAsync(string codeCountry)
		{
			endPoint = "rate";

			using (var client = new HttpClient())
			{
				var httpResponse = await client.GetAsync($"{baseUrl}{endPoint}?access_key={key}&country_code={codeCountry}");

				if (!httpResponse.IsSuccessStatusCode)
				{
					error = $"Get Rate By Country - Status Code : {httpResponse.StatusCode}";
					logger.LogError(error);
					throw new Exception(error);
				}

				var content = JsonConvert.DeserializeObject<dynamic>(await httpResponse.Content.ReadAsStringAsync());

				if (!((bool)content.success) || content.rates is null)
				{
					error = $"Get Rate By Country - {content.error.code} - {content.error.info}";
					logger.LogError(error);
					throw new Exception(error);
				}

				rate = JsonConvert.DeserializeObject<Rate>(await httpResponse.Content.ReadAsStringAsync());

				return rate;
			}
		}


		public async Task<IActionResult> List()
		{
			if (rateList is null)
			{
				try
				{
					rateList = await GetRateListAsync();
				}

				catch (Exception ex)
				{
					TempData["danger"] = ex.Message;
					return RedirectToAction("Index", "Home");
				}
			}

			return View(rateList.Rates);
		}

		public async Task<IActionResult> Details(string id)
		{
			if (String.IsNullOrEmpty(id))
			{
				error = $"This country is not found !";
				logger.LogError(error);
				TempData["danger"] = error;

				return RedirectToAction("Index", "Home");
			}

			if (rate is null)
			{
				try
				{
					rate = await GetRateByCountryAsync(id);
				}

				catch (Exception ex)
				{
					TempData["danger"] = ex.Message;
					return RedirectToAction("Index", "Home");
				}
			}

			return View(rate);
		}

		//[HttpPost]
		//public IActionResult GetRates([FromBody] string id)
		//{
		//	if (String.IsNullOrEmpty(id))
		//	{
		//		error = $"Rate List = code country null or empty";
		//		logger.LogError(error);
		//		TempData["danger"] = error;
		//		return RedirectToAction("Index", "Home");
		//	}

		//	return ViewComponent("GetRates", id);
		//}
	}
}
