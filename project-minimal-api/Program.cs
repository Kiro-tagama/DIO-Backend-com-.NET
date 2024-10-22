using Microsoft.EntityFrameworkCore;
using MinimalApi.DTOs;
using MinimalApi.Infrastructure.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbConnect>(options=>{
  options.UseMySql(
    builder.Configuration.GetConnectionString("mysqlConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysqlConnection"))
  );
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (LoginDTO loginDTO) =>{
  var userAdmin = loginDTO.Email == "adm@test.com" && loginDTO.Password == "123456";

  return userAdmin? Results.Ok("Login admin") : Results.Unauthorized(); 
});

app.Run();