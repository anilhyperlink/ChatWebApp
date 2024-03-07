using ChatWebApp.Models;
using ChatWebApp.Models.Comman;
using ChatWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebApp
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
            services.AddControllersWithViews();
            services.AddSession();

            // signalr service 
            services.AddSignalR();

            // app setting service 
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // remove AddAntiforgery cookies
            services.AddAntiforgery(options =>
            {
                options.Cookie.Expiration = TimeSpan.Zero; // Set the desired expiration time or a different value as needed
                options.Cookie.IsEssential = true; // Optional: Indicate whether the cookie is essential for the application's functionality
            });

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.JWTTokenGenKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            }).AddCookie(options =>
            {
                options.Cookie.Name = "Token"; // Set the cookie name
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Set to Always in production
                options.LoginPath = "/Account/SignIn"; // Set the login path
                options.SlidingExpiration = true; // Enable sliding expiration
            });

            // authentication service
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // account manage services
            services.AddScoped<IManageService<SignInModel, UserModel>, ManageService<SignInModel, UserModel>>();
            services.AddScoped<IManageService<SignUpModel, string>, ManageService<SignUpModel, string>>();

            // home manage service
            services.AddScoped<IManageService<string, UserListWithLastMessageModel>, ManageService<string, UserListWithLastMessageModel>>();
            services.AddScoped<IManageService<getUserMessage, List<UserListWithLastMessageModel>>, ManageService<getUserMessage, List<UserListWithLastMessageModel>>>();
            services.AddScoped<IManageService<MessageModel, string>, ManageService<MessageModel, string>>();
        
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

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=SignIn}/{id?}");
                // Map the SignalR hub
                endpoints.MapHub<ChatHub>("/chatHub");
            });

        }
    }
}
