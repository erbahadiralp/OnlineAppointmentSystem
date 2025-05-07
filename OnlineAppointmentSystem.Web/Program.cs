using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.Business.BackgroundServices;
using OnlineAppointmentSystem.Business.Extensions;
using OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework;
using OnlineAppointmentSystem.DataAccess.Extensions;
using OnlineAppointmentSystem.Entity.Concrete;
using System;

var builder = WebApplication.CreateBuilder(args);


// Loglama yapılandırması
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext yapılandırması DataAccess.Extensions'da yapıldığı için burada kaldırıldı
// builder.Services.AddDbContext<OnlineAppointmentSystemDbContext>...

// DataAccess katmanını kaydet - DbContext ve Repository'leri içerir
builder.Services.AddDataAccessServices(builder.Configuration);

// Business katmanını kaydet
builder.Services.AddBusinessServices();

// Add Identity

// Add Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true; // Bu ayar önemli
    
    // Kullanıcı adı ve email için normalizasyon ayarları
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
.AddEntityFrameworkStores<OnlineAppointmentSystemDbContext>()
.AddDefaultTokenProviders();


// Configure Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

// Background servisleri kaydet
builder.Services.AddHostedService<AppointmentReminderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();