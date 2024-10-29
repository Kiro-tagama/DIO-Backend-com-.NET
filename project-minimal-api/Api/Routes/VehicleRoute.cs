using MinimalApi.Domain.ModelViews;
using MinimalApi.DTOs;
using MinimalApi.Domain.Interface;
using MinimalApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MinimalApi.Route;
public class VehicleRoute{
  public static void MapVehicleRoute(IEndpointRouteBuilder route){
    
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

    route.MapPost("/vehicles", 
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
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
    .WithTags("Vehicle");

    route.MapGet("/vehicles", 
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
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
    .WithTags("Vehicle");

    route.MapGet("/vehicles/{id}", 
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
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
    .WithTags("Vehicle");

    route.MapPut("/vehicles/{id}", 
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
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("Vehicle");

    route.MapDelete("/vehicles/{id}", 
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
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("Vehicle");
  }
}