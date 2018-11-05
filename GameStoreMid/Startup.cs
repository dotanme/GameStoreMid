using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GameStoreMid.Data;
using GameStoreMid.Migrations;
using GameStoreMid.Models;
using GameStoreMid.Services;
using IgdbAPI;
using Newtonsoft.Json;
using unirest_net.http;

namespace GameStoreMid
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            IConfiguration GetSetting(IConfiguration config)
            {

                return config;
            }

            SettingFactory = () => GetSetting(configuration);
        }
        public static Func<IConfiguration> SettingFactory;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<MLApriori, MLApriori>();

            var authBuilder = services.AddAuthentication();

            if (!string.IsNullOrEmpty(Configuration["Authentication:Facebook:AppId"]))
            {
                authBuilder.AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                });
            }
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
          
          
            CreateRoles(serviceProvider);

        }


       

        private void CreateRoles(IServiceProvider serviceProvider)
        {

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            Task<IdentityResult> roleResult;
            string email = "MID@MID.com";
            ApplicationUser user;

            //Check that there is an Administrator role and create if not
            try
            {
                Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Administrator");
                hasAdminRole.Wait();

                if (!hasAdminRole.Result)
                {
                    roleResult = roleManager.CreateAsync(new IdentityRole("Administrator"));
                    roleResult.Wait();
                }

                //Check if the admin user exists and create it if not
                //Add to the Administrator role

                Task<ApplicationUser> testUser = userManager.FindByEmailAsync(email);
                testUser.Wait();
                user = testUser.Result;
                if (user == null)
                {
                    user = CreateAdminAsync(email, userManager).Result;
                }
                if (!userManager.IsInRoleAsync(user, "Administrator").Result)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(user, "Administrator");
                    newUserRole.Wait();
                }
            }
            catch (Exception ex)
            {

            }

        }

        private async Task<ApplicationUser> CreateAdminAsync(string email, UserManager<ApplicationUser> userManager)
        {
            Models.AccountViewModels.RegisterViewModel model = new Models.AccountViewModels.RegisterViewModel
            {
                City = "Raanana",
                Street = "asirey tsiyon",
                ConfirmPassword = "Qwe123!",
                Password = "Qwe123!",
                Country = "Israel",
                Email = email,
                ZipCode = 123456
            };
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Address = model.GetAddress() };
            var result = await userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                return user;
            }
            return null;

        }
    }
}
