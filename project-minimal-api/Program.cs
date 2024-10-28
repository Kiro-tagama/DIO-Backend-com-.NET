using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Interface;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MinimalApi.Route;
using Microsoft.OpenApi.Models;

// Settings for app
var builder = WebApplication.CreateBuilder(args);

var keyJwt = builder.Configuration.GetSection("Jwt").ToString();
if(string.IsNullOrEmpty(keyJwt)) keyJwt = "123456";

builder.Services.AddAuthentication(op=>{
  op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(op=>{
  op.TokenValidationParameters = new TokenValidationParameters{
    ValidateLifetime = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyJwt)),
    ValidateIssuer = false,
    ValidateAudience = false,
  };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministratorServices, AdministratorServices>();
builder.Services.AddScoped<IVehicleServices, VehicleServices>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(op=>{
  op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Insira o token JWT aqui"
  });
  op.AddSecurityRequirement(new OpenApiSecurityRequirement
  {{
    new OpenApiSecurityScheme{
      Reference = new OpenApiReference 
      {
        Type = ReferenceType.SecurityScheme,
        Id = "Bearer"
      }
    },
    new string[] {}
  }});
});

builder.Services.AddDbContext<DbConnect>(options=>{
  options.UseMySql(
    builder.Configuration.GetConnectionString("mysqlConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysqlConnection"))
  );
});

var app = builder.Build();

// Routes
HomeRoute.MapHomeRoute(app);
AdministratorRoute.MapAdminRoute(app,keyJwt);
VehicleRoute.MapVehicleRoute(app);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();