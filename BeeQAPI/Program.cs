using BAL.ContractIF;
using BAL.ContractIF.BAL.ContractIF;
using BAL.Implementation;
using BAL.Services;
using BeeQAPI.Authorization;
using DAL.ContractIF;
using DAL.ContractIF.DAL.ContractIF;
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
using BeeQAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);


// ===================== DB CONNECTION =====================

// 🔹 Dapper (Service Module)
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    return new MySqlConnection(connectionString);
});

// 🔹 Existing DBConnection (Auth Module)
builder.Services.AddScoped<DBConnection>();



// ===================== SERVICES =====================

// 🔹 Auth Module
builder.Services.AddScoped<IBAL_Auth, BAL_Auth>();
builder.Services.AddScoped<IDAL_Auth, DAL_Auth>();
builder.Services.AddScoped<IJwtService, JwtService>();

// 🔹 Service Module
builder.Services.AddScoped<IBAL_Service, BAL_Service>();
builder.Services.AddScoped<IDAL_Service, DAL_Service>();


// 🔥 REGISTER SERVICES
builder.Services.AddScoped<IBAL_Menu, BAL_Menu>();
builder.Services.AddScoped<IDAL_Menu, DAL_Menu>();

// 🔹 Organization Module

builder.Services.AddScoped<IBAL_Organization, BAL_Organization>();
builder.Services.AddScoped<IDAL_Organization, DAL_Organization>();

builder.Services.AddScoped<IBAL_BranchService, BAL_BranchService>();
builder.Services.AddScoped<IDAL_BranchService, DAL_BranchService>();





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
// BRANCH (UPDATED BASED ON YOUR FILES)
// ========================
builder.Services.AddScoped<IBAL_Branch, BAL_Branch>();
builder.Services.AddScoped<IDAL_Branch, DAL_Branch>();



//builder.Services.AddScoped<IBAL_User, BAL_User>();
//builder.Services.AddScoped<IDAL_User, DAL_User>();


// COUNTER 
// ========================
builder.Services.AddScoped<IDAL_Counter, DAL_Counter>();
builder.Services.AddScoped<IBAL_Counter, BAL_Counter>();

// 🔹 Service Module
builder.Services.AddScoped<IBAL_Service, BAL_Service>();
builder.Services.AddScoped<IDAL_Service, DAL_Service>();
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


builder.Services.AddControllers();


// ===================== SWAGGER =====================
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
        Description = "Enter 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                }
            },
            Array.Empty<string>()
        }
    });
});


// ===================== JWT AUTH =====================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

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


// ===================== AUTHORIZATION =====================
builder.Services.AddAuthorization();


builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>(); //Custom RBAC Policy Provider


// ===================== CORS =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// ===================== BUILD APP =====================
var app = builder.Build();


// ===================== MIDDLEWARE =====================
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