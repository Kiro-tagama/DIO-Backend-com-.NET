using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Interface;
using MinimalApi.Domain.Services;
using MinimalApi.Domain.ModelViews;
using MinimalApi.DTOs;
using MinimalApi.Infrastructure.Db;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorServices, AdministratorServices>();
builder.Services.AddScoped<IVehicleServices, VehicleServices>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbConnect>(options=>{
  options.UseMySql(
    builder.Configuration.GetConnectionString("mysqlConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysqlConnection"))
  );
});

var app = builder.Build();
#endregion

#region Home
//app.MapGet("/",()=> Results.Json(new Home()));
app.MapGet("/", async (DbConnect dbConnect) => {
  try{
    var canConnect = await dbConnect.Database.CanConnectAsync();
    return canConnect 
      ? Results.Ok(new { 
        message = "Servidor on", 
        detail = "Conexão com MySQL bem-sucedida.", 
        doc = new Home()
      }) 
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
#endregion

#region Vehicle
  app.MapPost("/vehicles", 
( 
  [FromBody] 
  VehicleDTO vehicleDTO,
  IVehicleServices vehicleServices
) =>{
  try{
    // var vehicle = new Vehicle{
    //   Name = VehicleDTO.Name,
    //   Mark = VehicleDTO.Mark,
    //   Year = VehicleDTO.Year
    // };

    // vehicleServices.AddVehicle();

    // return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
  }
  catch (System.Exception ex)
  {
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
});
#endregion

app.UseSwagger();
app.UseSwaggerUI();

app.Run();