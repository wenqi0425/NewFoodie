using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using NewFoodie.Models;
using NewFoodie.Services.Interfaces;
using NewFoodie.Services.EFServices;

namespace NewFoodie
{
    // initialize configurations 
    public class Startup
    {
        // (DI) IConfiguration loading configuration, JSON, etc.
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // To tell the container to produce instances by the runtime, methods to register services in container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("LocalConnection")));

            // AddTransient is an method of Microsoft.Extensions.DependencyInjection package.
            // Register a service as transient,
            // i.e., to create a new instance of the service with the dependency injection (DI) container.
            services.AddTransient<IRecipeService, EFRecipeService>();
            services.AddTransient<IRecipeItemService, EFRecipeItemService>();
            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<ISearchService, SearchService>();

            //Enables Razor Pages in the application. Registers routing, options with the dependency injection(DI) container and Pages views.
            services.AddRazorPages();

            // Identity: authentication and authorization 
            services.AddIdentity<AppUser, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole<int>>()
                .AddDefaultUI();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";

                // Lockout options:
                options.Lockout.AllowedForNewUsers = true;

                // if access attempt fail 3 times, then the accout will be locked by 10 mins. 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;
            });
        }

        // to configure the HTTP request pipeline by the runtime.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())  
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else   // IsEnvironment, IsProduction  
            {
                app.UseExceptionHandler("/Error");  
                app.UseHsts();  // for SSL security(https)
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();  // valid user (password, username, emailConfirmed?)
            app.UseAuthorization();   // Authorities, role

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
