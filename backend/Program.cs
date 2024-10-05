using backend.Models;
using backend.Repositories;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using backend.Interfaces;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// MongoDB configuration
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

var mongoConnectionString = builder.Configuration["MongoDbSettings:ConnectionString"];
var mongoDatabaseName = builder.Configuration["MongoDbSettings:DatabaseName"];

// Configure MongoDB Identity
builder.Services.AddIdentity<User, Role>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddMongoDbStores<User, Role, Guid>(mongoConnectionString, mongoDatabaseName)
    .AddDefaultTokenProviders();



// JWT configuration
var jwtKey = builder.Configuration["Jwt:Key"]; // Store your key in appsettings.json
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Change to true in production
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

// Add controllers or other services
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Add endpoints, middleware, etc.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
