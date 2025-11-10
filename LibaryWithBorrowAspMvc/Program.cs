using Domain.Common;
using LibaryWithBorrowAspMvc.Core;
using LibaryWithBorrowAspMvc.Core.Interfaces;
using LibaryWithBorrowAspMvc.Core.Services;
using LibaryWithBorrowAspMvc.Data;
using LibaryWithBorrowAspMvc.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Db conext service
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register generic repo for all TEntity
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<PasswordHasher<User>>();

// Cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // redirect if not logged in
        options.AccessDeniedPath = "/Account/AccessDenied"; // Path to AccessDenied page

        // Keep cookie after browser closes
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // e.g. 7 days
        options.Cookie.IsEssential = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();




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
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
