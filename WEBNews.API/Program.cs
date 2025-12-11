using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WEBNews.Data;
using WEBNews.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Ensure App_Data exists
var dataDir = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(dataDir);

// Add services to the container.
builder.Services.AddControllers();

// DbContext (SQLite file)
var conn = builder.Configuration.GetConnectionString("Default")!
    .Replace("App_Data", dataDir.Replace("\\", "/"));

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(conn);
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WEBNews API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[] {}
        }
    });
});

// JWT Auth
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Auto create database on startup (model snapshot)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}
app.UseSwagger();
app.UseSwaggerUI();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
