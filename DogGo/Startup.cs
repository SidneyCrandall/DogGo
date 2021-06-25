using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DogGo
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
            // Get the tranisent, but add the class
            services.AddControllersWithViews();
            // Used to display Walker
            services.AddTransient<IWalkerRepository, WalkerRepository>();
            // Added to have Owner display
            services.AddTransient<IOwnerRepository, OwnerRepository>();
            // Ability to view Dogs 
            services.AddTransient<IDogRepository, DogRepository>();
            // Same as above
            services.AddTransient<INeighborhoodRepository, NeighborhoodRepository>();
            // We are now incorporating The walks.
            services.AddTransient<IWalksRepository, WalksRepository>();
            // We wnat a user to have the ability to just change or view their own dogs and info. 
            // This helps to Authenticate the user and what they will be able to see
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/Owners/LogIn");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            // Authentication must come before the authorization. We need to 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
