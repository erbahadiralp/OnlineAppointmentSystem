// Program.cs - .NET 8 için
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Business.BackgroundServices;
using OnlineAppointmentSystem.Business.Concrete;
using OnlineAppointmentSystem.Business.Extensions;
using OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework;
using OnlineAppointmentSystem.DataAccess.Extensions;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Web.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Loglama yapılandırması
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.EnableEndpointRouting = true;
})
.AddViewLocalization()
.AddDataAnnotationsLocalization();

// DataAccess katmanını kaydet - DbContext ve Repository'leri içerir
builder.Services.AddDataAccessServices(builder.Configuration);

// Business katmanını kaydet
builder.Services.AddBusinessServices();

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
    
    // E-posta doğrulama ayarları - özel mantık AuthManager'da uygulanıyor
    options.SignIn.RequireConfirmedEmail = false; // AuthManager'da rol bazlı kontrol yapıyoruz
})
.AddEntityFrameworkStores<OnlineAppointmentSystemDbContext>()
.AddDefaultTokenProviders();

// Configure Identity Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // 7 günden 30 güne çıkarıldı

    // Cookie ayarlarını geliştirme ortamı için optimize ediyoruz
    if (builder.Environment.IsDevelopment())
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
    }
    else
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    }

    // Cookie olaylarını yapılandırıyoruz
    options.Events.OnRedirectToLogin = context =>
    {
        // API istekleri için 401 dönüş
        if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == 200)
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

// Background servisleri kaydet
builder.Services.AddHostedService<AppointmentReminderService>();

builder.Services.AddSingleton<EmailQueueService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<EmailQueueService>());

// Business Services
builder.Services.AddScoped<IAppointmentService, AppointmentManager>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<IEmailService, EmailManager>();
builder.Services.AddScoped<IEmployeeService, EmployeeManager>();
builder.Services.AddScoped<IServiceService, ServiceManager>();

// Diğer servis kayıtları...

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

// Middleware sıralaması kritik
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
