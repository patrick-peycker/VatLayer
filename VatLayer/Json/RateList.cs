using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace VatLayer.Json
{
	public class RateList
	{
		public bool Success { get; set; }
		public Dictionary<string, Rate> Rates { get; set; }
	}

}
