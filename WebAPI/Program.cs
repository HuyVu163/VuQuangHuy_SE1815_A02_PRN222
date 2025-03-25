using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebAPI.Hubs;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<NmsContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));
            builder.Services.Configure<AdminAccountConfig>(builder.Configuration.GetSection("AdminAccount"));
            builder.Services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Grandmas.Cookie";
                    config.LoginPath = "/Login";
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    config.Cookie.IsEssential = true;
                    config.SlidingExpiration = true;
                    config.AccessDeniedPath = "/AccessDenied";

                });
            builder.Services.AddControllers().AddJsonOptions(
                x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); 
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("0"));
                options.AddPolicy("LecturerPolicy", policy => policy.RequireRole("2"));
                options.AddPolicy("StaffPolicy", policy => policy.RequireRole("1"));

            });
            builder.Services.AddSignalR();


            builder.Services.AddSession();
            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            

            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NewsHub>("/newsHub");
                // Other endpoints...
            });

            app.Run();
        }
    }
}
