using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Interface;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MinimalApi.Route;
using Microsoft.OpenApi.Models;

using MinimalApi;

public class Startup{

  public IConfiguration Configuration { get;set; } = default!;
  public Startup(IConfiguration configuration){
    Configuration = configuration;
    keyJwt = Configuration.GetSection("Jwt").ToString() ?? "123456";
  }

  private string keyJwt;

  public void ConfigureServices(IServiceCollection services){
    services.AddAuthentication(op=>{
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

    services.AddAuthorization();

    services.AddScoped<IAdministratorServices, AdministratorServices>();
    services.AddScoped<IVehicleServices, VehicleServices>();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(op=>{
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

    services.AddDbContext<DbConnect>(options=>{
      options.UseMySql(
        Configuration.GetConnectionString("mysqlConnection"),
        ServerVersion.AutoDetect(Configuration.GetConnectionString("mysqlConnection"))
      );
    });
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseRouting();
    
    app.UseAuthentication();
    app.UseAuthorization();

    // Routes
    app.UseEndpoints(endpoints =>{
      HomeRoute.MapHomeRoute(endpoints);
      AdministratorRoute.MapAdminRoute(endpoints,keyJwt);
      VehicleRoute.MapVehicleRoute(endpoints);
    });
  }
}