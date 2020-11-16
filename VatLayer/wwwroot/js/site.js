
$("#list_CountrySelected").change(function ()
{
	let countryCode = $(this).children("option:selected").val();
	let countryName = $(this).children("option:selected").text();

	if (countryName === "All")
	{
		$("#list_a_details").removeClass();
		$("#list_a_details").addClass("btn btn-success disabled");
		$("#list-a-details").attr("href", "/vat/details");
	}

	else
	{
		let route = "/vat/details/" + countryCode;
		$("#list_a_details").removeClass();
		$("#list_a_details").addClass("btn btn-success");
		$("#list-a-details").attr("href", route);
	}



	//$("#list_btn_details").prop("asp-route-id", countryName === "All" ? "" : countryCode);

	$("#list_CountriesTable tr.country").filter(function ()
	{
		$(this).toggle($(this).text().indexOf(countryName === "All" ? "" : countryName) > -1)
	});




	//$.ajax(
	//	{
	//		type: 'Post',
	//		url: '/Vat/GetRates',
	//		contentType: 'application/json; charset=utf-8',
	//		data: JSON.stringify(codeCountry),
	//		dataType: 'html',

	//		complete: function (view, statut)
	//		{
	//			$("#rateListSection").html("<br/>")
	//			$("#rateListSection").append(view.responseText);
	//		},

	//		error: function (resultat, status, error)
	//		{
	//			alert(error);
	//		}
	//	});
});
