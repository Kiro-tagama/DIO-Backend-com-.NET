using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Interface;
using MinimalApi.Domain.Services;
using MinimalApi.DTOs;
using MinimalApi.Infrastructure.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorServices, AdministratorServices>();

builder.Services.AddDbContext<DbConnect>(options=>{
  options.UseMySql(
    builder.Configuration.GetConnectionString("mysqlConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysqlConnection"))
  );
});

var app = builder.Build();

app.MapGet("/", async (DbConnect dbConnect) => {
  try{
    var canConnect = await dbConnect.Database.CanConnectAsync();
    return canConnect 
      ? Results.Ok(new { message = "Servidor on", detail = "Conexão com MySQL bem-sucedida." }) 
      : Results.Json(new { message = "Falha ao conectar ao MySQL." }, statusCode: 500);
  }
  catch (Exception ex){
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
});

app.MapPost("/login", 
( 
  [FromBody] 
  LoginDTO loginDTO,
  IAdministratorServices administratorServices
) =>{
  try{
    var userAdmin = administratorServices.Login(loginDTO) != null;

    return userAdmin? 
      Results.Ok("Login admin") 
      : Results.Unauthorized(); 
  }
  catch (System.Exception ex)
  {
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
});

app.Run();