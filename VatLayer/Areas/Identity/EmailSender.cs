using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VatLayer.Areas.Identity
{
	public class EmailSender : IEmailSender
	{
		private readonly string apiKey;
		private readonly string fromName;
		private readonly string fromEmail;

		public EmailSender(IConfiguration configuration)
		{
			this.apiKey = configuration["SendGrid:key"];
			this.fromName = configuration["SendGrid:name"];
			this.fromEmail = configuration["SendGrid:from"];
		}

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress(fromEmail, fromName),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message

			};

			msg.AddTo(new EmailAddress(email));

			msg.SetClickTracking(false, false);

			await client.SendEmailAsync(msg);
		}
	}
}
