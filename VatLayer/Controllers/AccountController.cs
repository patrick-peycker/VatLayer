using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using VatLayer.Models;

namespace VatLayer.Controllers
{
	[Authorize(Roles = "Administrator")]
	public class AccountController : Controller
	{
		private readonly UserManager<User> userManager;
		private readonly RoleManager<Role> roleManager;

		public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
		}

		public IActionResult Get()
		{
			IEnumerable<User> users = userManager.Users;

			return View(users);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			User user = await userManager.FindByIdAsync(id.ToString());

			if (user is null)
			{
				return RedirectToAction("GetUsers");
			}

			var claims = await userManager.GetClaimsAsync(user);

			Console.WriteLine(claims.Select(x => x.Value).ToList());

			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([Bind("Id, FirstName, LastName, DateOfBirth, UserName, Email")] User user)
		{
			if (!ModelState.IsValid)
				return View(user);

			var result = await userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				return RedirectToAction("get");
			}

			return View(user);
		}

		public IActionResult Subscription()
		{
			return View(new Subscription());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Subscription(Subscription subscription)
		{
			if (!ModelState.IsValid)
			{
				return View(subscription);
			}

			var user = new User
			{
				Email = subscription.Email,
				UserName = subscription.Email,
				FirstName = subscription.FirstName,
				LastName = subscription.LastName
			};

			var resultUser = await this.userManager.CreateAsync(user, subscription.Password);

			if (resultUser.Succeeded)
			{
				var resultRole = await this.userManager.AddToRoleAsync(user, subscription.RoleSelected);

				if (resultRole.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}

				else
				{
					foreach (var error in resultRole.Errors)
					{
						ModelState.AddModelError(error.Code, error.Description);
					}

					return View(subscription);
				}
			}
			else
			{
				foreach (var error in resultUser.Errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
				}

				return View(subscription);
			}
		}
	}
}
