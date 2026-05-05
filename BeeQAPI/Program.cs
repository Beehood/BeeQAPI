
using BAL.ContractIF;
using BAL.Implementation;
using BAL.Services;
using BeeQAPI.Authorization;
using BeeQAPI.Middleware;
using DAL.ContractIF;
using DAL.ContractIF;
using DAL.Dbcontext;
using DAL.Implementation;
using DAL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===================== DB CONNECTION =====================

// 🔹 Dapper (Service Module)
//builder.Services.AddScoped<IDbConnection>(sp =>
//{
//    var configuration = sp.GetRequiredService<IConfiguration>();
//    var connectionString = configuration.GetConnectionString("DefaultConnection");

//    return new MySqlConnection(connectionString);
//});


builder.Services.AddScoped<DBConnection>();

// ========================
// AUTH
// ========================
builder.Services.AddScoped<IBAL_Auth, BAL_Auth>();
builder.Services.AddScoped<IDAL_Auth, DAL_Auth>();

// ========================
// ORGANIZATION
// ========================
builder.Services.AddScoped<IBAL_Organization, BAL_Organization>();
builder.Services.AddScoped<IDAL_Organization, DAL_Organization>();

// ========================
// BRANCH 
// ========================
builder.Services.AddScoped<IBAL_Branch, BAL_Branch>();
builder.Services.AddScoped<IDAL_Branch, DAL_Branch>();

// ========================
// COUNTER 
// ========================
builder.Services.AddScoped<IDAL_Counter, DAL_Counter>();
builder.Services.AddScoped<IBAL_Counter, BAL_Counter>();


// ========================
// SERVICE
// ========================
builder.Services.AddScoped<IBAL_Service, BAL_Service>();
builder.Services.AddScoped<IDAL_Service, DAL_Service>();

// ========================
// BRANCH SERVICE
// ========================
builder.Services.AddScoped<IBAL_BranchService, BAL_BranchService>();
builder.Services.AddScoped<IDAL_BranchService, DAL_BranchService>();

// ========================
// COUNTER SERVICE
// ========================
builder.Services.AddScoped<IBAL_CounterService, BAL_CounterService>();
builder.Services.AddScoped<IDAL_CounterService, DAL_CounterService>();


// ========================
// USER
// ========================
builder.Services.AddScoped<IBAL_User, BAL_User>();
builder.Services.AddScoped<IDAL_User, DAL_User>();

//=========================
// ROLE
//=========================
builder.Services.AddScoped<IBAL_Role, BAL_Role>();
builder.Services.AddScoped<IDAL_Role, DAL_Role>();

// =========================
//  PERMISSION
// =========================
builder.Services.AddScoped<IBAL_Permission, BAL_Permission>();
builder.Services.AddScoped<IDAL_Permission, DAL_Permission>();

// ========================
// JWT
// ========================
builder.Services.AddScoped<IJwtService, JwtService>();
//========================
// MENU
//=========================
builder.Services.AddScoped<IBAL_Menu, BAL_Menu>();
 builder.Services.AddScoped<IDAL_Menu, DAL_Menu>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Queue Management API",
        Version = "v1"
    });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: Bearer eyJhbGciOiJIUzI1...",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Optional: good for development
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),

            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

// Custom policy provider for permission-based authorization
builder.Services.AddSingleton<IAuthorizationPolicyProvider,
    PermissionPolicyProvider>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");
app.UseMiddleware<GlobalExceptionMiddleware>();  //For Global Exception Handling 


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
