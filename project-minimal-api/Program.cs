using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Interface;
using MinimalApi.Domain.Services;
using MinimalApi.Domain.ModelViews;
using MinimalApi.DTOs;
using MinimalApi.Infrastructure.Db;
using MinimalApi.Domain.Entities;

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
})
.WithTags("Home");

app.MapPost("/administrator/login", 
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
})
.WithTags("Administrator");
#endregion

#region Vehicle
app.MapPost("/vehicles", 
(
  [FromBody]
  VehicleDTO vehicleDTO,
  IVehicleServices vehicleServices
) => {
  try{
    var vehicle = new Vehicle{
      Name = vehicleDTO.Name,
      Mark = vehicleDTO.Mark,
      Year = vehicleDTO.Year
    };

    vehicleServices.AddVehicle(vehicle);

    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
  }
  catch (System.Exception ex)
  {
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
})
.WithTags("Vehicle");

app.MapGet("/vehicles", 
(
  [FromQuery]
  int? page,
  IVehicleServices vehicleServices
) => {
  try{
    var vehicles = vehicleServices.GetVehicles(page);

    return vehicles.Count() > 0 
      ? Results.Ok(vehicles) 
      : Results.NotFound();
  }
  catch (System.Exception ex)
  {
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
})
.WithTags("Vehicle");

app.MapGet("/vehicles/{id}", 
(
  [FromRoute]
  int id,
  IVehicleServices vehicleServices
) => {
  try{
    var vehicle = vehicleServices.GetVehicleById(id);

    return vehicle != null 
      ? Results.Ok(vehicle) 
      : Results.NotFound();
  }
  catch (System.Exception ex)
  {
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
})
.WithTags("Vehicle");

app.MapPut("/vehicles/{id}", 
(
  [FromRoute]
  int id,
  VehicleDTO vehicleDTO,
  IVehicleServices vehicleServices
) => {
  try{
    var vehicle = vehicleServices.GetVehicleById(id);

    if(vehicle == null) return Results.NotFound();

    vehicle.Name = vehicleDTO.Name;
    vehicle.Mark = vehicleDTO.Mark;
    vehicle.Year = vehicleDTO.Year;

    vehicleServices.UpdateVehicle(vehicle);

    return Results.Ok(vehicle); 
  }
  catch (System.Exception ex)
  {
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
})
.WithTags("Vehicle");

app.MapDelete("/vehicles/{id}", 
(
  [FromRoute]
  int id,
  IVehicleServices vehicleServices
) => {
  try{
    var vehicle = vehicleServices.GetVehicleById(id);
    if (vehicle == null) return Results.NotFound();

    vehicleServices.DeleteVehicle(vehicle);

    return Results.Ok(vehicle);
  }
  catch (System.Exception ex)
  {
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
})
.WithTags("Vehicle");
#endregion

app.UseSwagger();
app.UseSwaggerUI();

app.Run();