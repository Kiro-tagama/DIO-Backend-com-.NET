using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Interface;
using MinimalApi.Domain.Services;
using MinimalApi.Domain.ModelViews;
using MinimalApi.DTOs;
using MinimalApi.Infrastructure.Db;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Enuns;
using MinimalApi.Dominio.ModelViews;

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

app.MapPost("/administrator", 
( 
  [FromBody] 
  AdministratorDTO administratorDTO,
  IAdministratorServices administratorServices
) => {
  try{
    var validation = new ValidationErr{
      Messages = new List<string>()
    };

    if(string.IsNullOrEmpty(administratorDTO.Email)) validation.Messages.Add("O email não pode ser vazio");
    if(string.IsNullOrEmpty(administratorDTO.Password)) validation.Messages.Add("A senha não pode ser vazia");
    if(administratorDTO.Profile == null) validation.Messages.Add("O profile não pode ser vazio");

    if(validation.Messages.Count > 0) return Results.BadRequest(validation);

    var adm = new Administrator{
      Email = administratorDTO.Email,
      Password = administratorDTO.Password,
      Profile = administratorDTO.Profile.ToString() ?? Profile.Editor.ToString()
    };

    administratorServices.AddAdministrator(adm);

    return Results.Created($"/admin/{adm.Id}", new AdministradorModelView {
        Id = adm.Id,
        Email = adm.Email,
        Profile = adm.Profile
      });
  }
  catch (System.Exception ex)
  {
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
})
.WithTags("Administrator");

app.MapGet("/administrator", 
(
  [FromQuery]
  int? page,
  IAdministratorServices administratorServices
) => {
  try{
    var adms = new List<AdministradorModelView>();
    var listAdms = administratorServices.GetAllAdministrators(page);

    foreach (var adm in listAdms){
      adms.Add(new AdministradorModelView{
        Id = adm.Id,
        Email = adm.Email,
        Profile = adm.Profile
      });
    }

    return adms.Count() > 0 
      ? Results.Ok(adms) 
      : Results.NotFound();
  }
  catch (System.Exception ex)
  {
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
})
.WithTags("Administrator");

app.MapGet("/administrator/{id}", 
(
  [FromQuery]
  int id,
  IAdministratorServices administratorServices
) => {
  try{
    var adm = administratorServices.GetAdministratorById(id);

    // formated data
    return adm != null 
      ? Results.Ok(new AdministradorModelView {
        Id = adm.Id,
        Email = adm.Email,
        Profile = adm.Profile
      }) 
      : Results.NotFound();
  }
  catch (System.Exception ex)
  {
    return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
  }
})
.WithTags("Administrator");
#endregion

#region Vehicle

ValidationErr validDTO(VehicleDTO vehicleDTO){
    var validation = new ValidationErr{
        Messages = new List<string>()
    };

    if(string.IsNullOrEmpty(vehicleDTO.Name))
        validation.Messages.Add("O nome não pode ser vazio");

    if(string.IsNullOrEmpty(vehicleDTO.Mark))
        validation.Messages.Add("A Marca não pode ficar em branco");

    if(vehicleDTO.Year < 1950)
        validation.Messages.Add("Veículo muito antigo, aceito somete anos superiores a 1950");

    return validation;
}

app.MapPost("/vehicles", 
(
  [FromBody]
  VehicleDTO vehicleDTO,
  IVehicleServices vehicleServices
) => {
  try{
    var validation = validDTO(vehicleDTO);
    if(validation.Messages.Count > 0)
      return Results.BadRequest(validation);

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
    
    var validation = validDTO(vehicleDTO);
    if(validation.Messages.Count > 0)
      return Results.BadRequest(validation);

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