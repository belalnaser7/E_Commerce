using ECommerce.Api.Middleware;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mapping;
using ECommerce.Application.Police;
using ECommerce.Application.Services;
using ECommerce.Domain.Domain_Models;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddScoped<IRepositoryProduct, RepositoryProduct>();
builder.Services.AddScoped<IServicesProduct, ServicesProduct>();
builder.Services.AddScoped<IRepositoryCategory, RepositoryCategory>();
builder.Services.AddScoped<IServicesCategory, ServicesCategory>();
builder.Services.AddScoped<IServicesCart, ServicesCart>();
builder.Services.AddScoped<IRepositoryCart, RepositoryCart>();
builder.Services.AddScoped<IRepositoryOrder, RepositoryOrder>();
builder.Services.AddScoped<IServicesOrder, ServicesOrder>();
builder.Services.AddScoped<IServicesAuz, ServicesAuz>();
//builder.Services.AddScoped<AuthorizationHandler<CanDeleteProductRequirement,Product>, CanDeleteProductHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CanDeleteOrUpdateProductHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CanViewCartHandler>();
builder.Services.AddScoped<IAuthorizationHandler, AccessUserOnHisOrderHandler>();



MapsterConfig.RegisterMappings();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ECommerce API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT Token"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
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

// DbContext
builder.Services.AddDbContext<E_commerceDbcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
});

// 🔥 Identity لازم تضيفه
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<E_commerceDbcontext>()
    .AddDefaultTokenProviders();

var key = Encoding.UTF8.GetBytes(
    builder.Configuration["Jwt:Key"]!
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(key)
        };
});

builder.Services.AddAuthorization(policy =>
{
    policy.AddPolicy("CanManageProducts", policy => policy.RequireRole("Admin", "Seller"));
});
builder.Services.AddAuthorization(policy =>
{
    policy.AddPolicy("CanShow", p => p.RequireRole("Admin", "Customer"));
});

//serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt",
    rollingInterval: RollingInterval.Day,
    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] UserId={UserId} Path={RequestPath} Message={Message}{NewLine}")
    .CreateLogger();
builder.Host.UseSerilog();
var app = builder.Build();

using (var scope = app.Services.CreateScope()) // هنا بحجز حاجه اسمها scope وانا اصلا مش فاهمها 
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();  // هنا بجيب inject من ال RoleManger 
    string[] roles = { "Admin", "Customer", "Seller" };
    foreach (var item in roles)
    {
        var roleExist = await roleManager.RoleExistsAsync(item);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(item));
        }
    }
}


// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<SerilogEnrichmentMiddleware>();
app.MapControllers();

app.Run();


// 🔥 هنا نعمل user seed (بعد build)
//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//    var user = await userManager.FindByEmailAsync("admin@test.com");

//    if (user == null)
//    {
//        var newUser = new ApplicationUser
//        {
//            UserName = "admin@test.com",
//            Email = "admin@test.com",
//            EmailConfirmed = true
//        };

//        await userManager.CreateAsync(newUser, "Admin@123");
//    }
//}