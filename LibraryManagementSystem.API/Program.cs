using LibraryManagementSystem.API.Services;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagementSystem.Data.DTO;
using LibraryManagmentSystem.Data;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Force a test log
    logger.Info("✅ Test log: Application started");

    // Configure logging
    builder.Logging.ClearProviders();
    builder.Host.UseNLog(); // Add NLog back to the logging pipeline

    builder.Services.AddScoped<IBookService, BookService>();

    // Add services to the container
    builder.Services.AddControllers();

    // Add OpenAPI services (required for Scalar)
    builder.Services.AddOpenApi();

    // Add DbContext
    builder.Services.AddDbContext<LibraryContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

    // Add repository pattern
    builder.Services.AddScoped(typeof(ILibraryGenericRepo<>), typeof(LibraryGenericRepo<>));
    builder.Services.AddAutoMapper(typeof(BookAutoMapperProfile));
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi(); // Maps the OpenAPI specification
        app.MapScalarApiReference(); // Maps Scalar UI (default: /scalar)
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    logger.Error(e, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}