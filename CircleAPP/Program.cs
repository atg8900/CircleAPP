using CircleApp.Data.Helpers;
using CircleApp.Data.Models;
using CircleApp.Data.Services;
using CircleAPP.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CircleAPP
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IHashtagService, HashtagService>();
            builder.Services.AddScoped<IStoriesService, StoriesService>();
            builder.Services.AddScoped<IFilesService, FilesService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IFriendsService, FriendsService>();

            //Identity configuration
            builder.Services.AddIdentity<User, IdentityRole<int>>(
                options =>
                {
                    //Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 4;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Authentication/Login";
                options.AccessDeniedPath = "/Authentication/AccessDenied";
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Auth:Google:ClientId"] ?? "";
                    options.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"] ?? "";
                    options.CallbackPath = "/signin-google";
                }).AddGitHub(options =>
                {
                    options.ClientId = builder.Configuration["Auth:GitHub:ClientId"] ?? "";
                    options.ClientSecret = builder.Configuration["Auth:GitHub:ClientSecret"] ?? "";
                    //options.Scope.Add("user:email");
                    //options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    options.CallbackPath = "/signin-github";
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();
            //seed data
            using (var scope = app.Services.CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbcontext.Database.MigrateAsync();
                await DbInitializer.SeedAsync(dbcontext);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                await DbInitializer.SeedUsersAndRolesAsync(userManager, roleManager);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
