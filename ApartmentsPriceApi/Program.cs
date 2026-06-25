using ApartmentsPriceApi.Data;
using ApartmentsPriceApi.Services.ApartmentPrice.Interfaces;
using ApartmentsPriceApi.Services.ApartmentPrice.Services;
using ApartmentsPriceApi.Services.EmailNotification;
using ApartmentsPriceApi.Services.EmailNotification.Interfaces;
using ApartmentsPriceApi.Services.EmailNotification.Services;
using ApartmentsPriceApi.Services.PriceChecker.Interfaces;
using ApartmentsPriceApi.Services.PriceChecker.Services;
using ApartmentsPriceApi.Services.PriceMonitor.Services;
using ApartmentsPriceApi.Services.SubscriptionService.Interfaces;
using ApartmentsPriceApi.Services.SubscriptionService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApartmentsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();

builder.Services.AddHttpClient<IApartmentPriceService, ApartmentPriceService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler 
        { 
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IPriceCheckerService, PriceCheckerService>();

builder.Services.AddHostedService<PriceMonitorService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app);

app.Run();
