using DeliverySystemBackend;
using DeliverySystemBackend.Controller;
using Microsoft.EntityFrameworkCore;
using DeliverySystemBackend.Repository;
using DeliverySystemBackend.Service.DomainServices;
using FluentValidation;
using DeliverySystemBackend.Service.DomainModels;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddDbContext<DeliveryContext>((serviceProvider, options) =>
{
    var dbSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    options.UseNpgsql(dbSettings.ConnectionString);
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddTransient<IValidator<Order>, OrderValidator>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowLocalhost3000");

app.MapControllers();

app.Run();
