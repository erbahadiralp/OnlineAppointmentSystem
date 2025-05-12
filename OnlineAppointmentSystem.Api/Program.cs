using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Business.BackgroundServices;
using OnlineAppointmentSystem.Business.Concrete;
using OnlineAppointmentSystem.Business.Mapping;
using OnlineAppointmentSystem.DataAccess.Cache;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<OnlineAppointmentSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<OnlineAppointmentSystemDbContext>()
    .AddDefaultTokenProviders();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Online Appointment System API", Version = "v1" });
    
    // JWT Authentication için Swagger yapılandırması
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Cache Services
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "OnlineAppointmentSystem_";
});

// Business Services
builder.Services.AddScoped<IAppointmentService, AppointmentManager>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<IEmailService, EmailManager>();
builder.Services.AddScoped<IEmployeeService, EmployeeManager>();
builder.Services.AddScoped<IServiceService, ServiceManager>();
builder.Services.AddScoped<EmailQueueService>();
builder.Services.AddHostedService<EmailQueueService>();
// Diğer servisler
//
// builder.Services.AddScoped<IDoctorService, DoctorManager>();
// builder.Services.AddScoped<IPatientService, PatientManager>();

// Cache Services
builder.Services.AddScoped<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();

builder.Services.AddScoped<INotificationService, NotificationManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS
app.UseCors("AllowAll");

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run(); 