using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using VatLayer.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VatLayer.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using VatLayer.Areas.Identity;

namespace VatLayer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));

			// Switch On - Role

			//services.AddDefaultIdentity<User>(options => {
			services.AddIdentity<User, Role>(options =>
			{
				options.SignIn.RequireConfirmedAccount = true;
				options.User.RequireUniqueEmail = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
			})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultUI()
				.AddDefaultTokenProviders();

			services.AddAuthentication()
				.AddMicrosoftAccount(o =>
				{
					o.ClientId = Configuration["MicrosoftConnect:key"];
					o.ClientSecret = Configuration["MicrosoftConnect:secret"];
				})

				.AddGoogle(o =>
				{
					o.ClientId = Configuration["GoogleConnect:key"];
					o.ClientSecret = Configuration["GoogleConnect:secret"];
				});

			services.AddTransient<IEmailSender, EmailSender>();

			services.AddControllersWithViews();
			services.AddRazorPages().AddNewtonsoftJson();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});

			CreateRoles(serviceProvider);

		}

		private void CreateRoles(IServiceProvider serviceProvider)
		{
			var rolesManager = serviceProvider.GetService<RoleManager<Role>>();
			var roles = Role.Roles;

			foreach (var roleName in roles)
			{
				var roleExists = rolesManager.RoleExistsAsync(roleName)
					.GetAwaiter()
					.GetResult();

				if (!roleExists)
				{
					rolesManager.CreateAsync(new Role { Name = roleName })
						 .GetAwaiter()
						 .GetResult();
				}
			}
		}
	}
}
