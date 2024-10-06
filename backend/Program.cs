using backend.Models;
using backend.Repositories;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using backend.Interfaces;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Register Cloudinary settings and service
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<CloudinaryService>();

// MongoDB configuration
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

var mongoSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

// Register MongoDB client and database
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoSettings.ConnectionString));
builder.Services.AddScoped<IMongoDatabase>(provider =>
    provider.GetService<IMongoClient>().GetDatabase(mongoSettings.DatabaseName));

// Configure MongoDB Identity
builder.Services.AddIdentity<User, Role>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddMongoDbStores<User, Role, Guid>(mongoSettings.ConnectionString, mongoSettings.DatabaseName)
.AddDefaultTokenProviders();

// JWT configuration
var jwtKey = builder.Configuration["Jwt:Key"]; // Store your key in environment variables in production
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true; // Set to true for production
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Register your services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<VendorService>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();

builder.Services.AddAuthorization();

// Add controllers or other services
builder.Services.AddControllers();

// CORS setup for development (Allow all origins)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()   // Allow requests from any origin
               .AllowAnyMethod()   // Allow all HTTP methods (GET, POST, PUT, etc.)
               .AllowAnyHeader();  // Allow all headers
    });
});

// In production, replace "AllowAll" with a more restrictive CORS policy
/*
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("https://yourdomain.com", "https://anotherdomain.com") // Allow specific domains
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
*/

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Add endpoints, middleware, etc.
app.UseCors("AllowAll"); // Apply the "AllowAll" CORS policy

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
