using System.Text.Json.Serialization;
using DeliverySystemBackend;
using DeliverySystemBackend.Controller.Validators;
using Microsoft.EntityFrameworkCore;
using DeliverySystemBackend.Repository;
using DeliverySystemBackend.Repository.Repositories;
using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainServices;
using DeliverySystemBackend.Service.DomainServices.Interfaces;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddDbContext<DeliveryDbContext>((serviceProvider, options) =>
{
    var dbSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    options.UseNpgsql(dbSettings.ConnectionString);
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICargoOwnerRepository, CargoOwnerRepository>();
builder.Services.AddScoped<ICarrierRepository, CarrierRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICargoRepository, CargoRepository>();
builder.Services.AddScoped<ITransportRepository, TransportRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICargoOwnerService, CargoOwnerService>();
builder.Services.AddScoped<ICarrierService, CarrierService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddTransient<OrderValidator>();
builder.Services.AddTransient<CargoOwnerValidator>();
builder.Services.AddTransient<CarrierValidator>();
builder.Services.AddTransient<UserValidator>();
builder.Services.AddTransient<CargoValidator>();
builder.Services.AddTransient<TransportValidator>();
builder.Services.AddTransient<AddressValidator>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "AuthCookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.LoginPath = "/api/auth/login";
        options.AccessDeniedPath = "/api/auth/accessdenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CargoOwnerOnly", policy => policy.RequireRole("CargoOwner"));
    options.AddPolicy("CarrierOnly", policy => policy.RequireRole("Carrier"));
});

var app = builder.Build();

app.UseCors("AllowLocalhost3000");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

