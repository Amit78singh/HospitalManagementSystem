using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// 🔹 1. Add DbContext
builder.Services.AddDbContext<HospitalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 2. Add CORS policy for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // 👈 your Angular frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 🔹 3. Add Controllers + JSON PascalCase for Angular model compatibility
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // PascalCase for API response
    });

// 🔹 4. Add Swagger with JWT Support
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HospitalManagementAPI",
        Version = "v1",
        Description = "Hospital Management System API with JWT Authentication"
    });

    // 🔐 JWT Bearer Auth in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer {your JWT token}'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });

 
});

// 🔹 5. JWT Authentication Config
var jwtKey = "ThisIsMySecretKeyForJwtTokenDoNotShareIt"; // 📌 Move to appsettings.json in real apps

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
              RoleClaimType = ClaimTypes.Role
        };
    });

// 🔹 6. Authorization
builder.Services.AddAuthorization();

// ✅ Build app
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

        var services = scope.ServiceProvider;
        var context= services.GetRequiredService<HospitalContext>();
        SeedRoles(context);

}

    // 🔹 7. Middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "HospitalManagementAPI v1");
            options.RoutePrefix = "swagger"; // available at /swagger
        });
    }

app.UseCors("AllowAngularApp"); // allow Angular to call API

app.UseHttpsRedirection();

app.UseAuthentication(); // ✅ Add Auth middleware before controllers
app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HospitalContext>();

    // ✅ Check if Admin already exists
    if (!context.Users.Any(u => u.Email == "admin@hospital.com"))
    {
        var admin = new User
        {
            Email = "admin@hospital.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin"
        };

        context.Users.Add(admin);
        context.SaveChanges();
        Console.WriteLine("✅ Seed admin user created.");
    }
}


app.Run();




// 🔹 8. Seed Roles
static void SeedRoles(HospitalContext context)
{
    var existingRoles=context.Users.Select(u=> u.Role).Distinct().ToList();
    var roles =new List<string> { "Admin", "User"};

    foreach (var role in roles)
    {
        if (!existingRoles.Contains(role))
        {
            context.Users.Add(new User
            {
                Email = $"{role.ToLower()}@hospital.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Default@123"),
                Role = role
            });
        }
    }
    context.SaveChanges();
}