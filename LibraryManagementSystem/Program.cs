using LibraryManagementSystem.API.Exceptions;
using LibraryManagementSystem.API.Services;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagementSystem.Data.DTO.AutoMapper;
using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Data.Identity;
using LibraryManagmentSystem.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Playwright;
using NLog;
using NLog.Web;
using Scalar.AspNetCore;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;


var logger = LogManager.Setup().LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();
logger.Debug("init main");


try
{
    var builder = WebApplication.CreateBuilder(args);

    var apiKey = builder.Configuration["Mailjet:ApiKey"];
    var secretKey = builder.Configuration["Mailjet:SecretKey"];

    var token = builder.Configuration["JwtToken:Token"];
    var issuer = builder.Configuration["JwtToken:Issuer"];
    var audience = builder.Configuration["JwtToken:Audience"];



    // Force a test log
    logger.Info("✅ Test log: Application started");

    // Configure logging


    builder.Logging.ClearProviders();
    builder.Host.UseNLog(); // Add NLog back to the logging pipeline

    builder.Services.AddScoped(typeof(LibraryGenericRepo<>));
    builder.Services.AddScoped<IBookService, BookService>();
    builder.Services.AddScoped<IAuthorService, AuthorService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IPublisherService, PublisherService>();
    builder.Services.AddScoped<IAuthorImageService, AuthorImageService>();
    builder.Services.AddScoped<IBookImageService, BookImagesService>();
    builder.Services.AddScoped<IBookRentalService, BookRentalServices>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IReturnedRentalService, ReturnedRentalService>();
    builder.Services.AddScoped<IPdfService, PdfService>();
    builder.Services.AddScoped<JWTService>();
    builder.Services.AddScoped<EmailService>();
    builder.Services.AddTransient<GlobalExceptionHandlerMiddleWare>();
    builder.Services.AddScoped<ImageServiceAuthor>();
    builder.Services.AddScoped<ImageServiceBook>();
    builder.Services.AddScoped<ExcelExportService>();
    builder.Services.AddScoped <CleanupService>();


    // Add services to the container
    builder.Services.AddControllers();

    // Add OpenAPI services (required for Scalar)
    builder.Services.AddOpenApi();

    // Add DbContext
    builder.Services.AddDbContext<LibraryContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

    builder.Services.AddIdentityCore<User>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedEmail = true;
    })
        .AddRoles<Role>()
        .AddRoleManager<RoleManager<Role>>()
        .AddEntityFrameworkStores<LibraryContext>()
        .AddSignInManager<SignInManager<User>>()
        .AddUserManager<UserManager<User>>()
        .AddDefaultTokenProviders();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateIssuer = true,
                ValidateAudience = false
            };
        });

    builder.Services.AddScoped(typeof(ILibraryGenericRepo<>), typeof(LibraryGenericRepo<>));
    builder.Services.AddAutoMapper(typeof(LibraryAutoMapperProfile));
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration).CreateLogger();


    builder.Services.AddSingleton<Microsoft.Playwright.IPlaywright>(sp =>
    {
        return Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
    });
    builder.Services.AddSingleton<Microsoft.Playwright.IBrowser>(sp =>
    {
        var playwright = sp.GetRequiredService<Microsoft.Playwright.IPlaywright>();
        return playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        }).GetAwaiter().GetResult();
    });
        var app = builder.Build();

    app.UseMiddleware<GlobalExceptionHandlerMiddleWare>();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }
    app.UseHttpsRedirection();
    app.UseAuthentication();
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