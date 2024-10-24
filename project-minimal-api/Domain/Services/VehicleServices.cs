using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interface;
using MinimalApi.DTOs;
using MinimalApi.Infrastructure.Db;

namespace MinimalApi.Domain.Services;

public class VehicleServices : IVehicleServices{

    private readonly DbConnect _connect;
    public VehicleServices(DbConnect connect){
      _connect = connect;
    }

    public void AddVehicle(Vehicle vehicle)
    {
      _connect.Vehicles.Add(vehicle);
      _connect.SaveChanges();
    }

    public void DeleteVehicle(Vehicle vehicle)
    {
      _connect.Vehicles.Remove(vehicle);
      _connect.SaveChanges();
    }

    public Vehicle? GetVehicleById(int id)
    {
      return _connect.Vehicles.Where(x => x.Id == id).FirstOrDefault();
    }

    public List<Vehicle> GetVehicles(int? page = 1, string? name = null, string? mark = null)
    {
      var query = _connect.Vehicles.AsQueryable();

      if (!string.IsNullOrWhiteSpace(name))
        query = query.Where(v => EF.Functions.Like(v.Name.ToLower(), $"%{name}%"));

      int itemsPerPage = 10;

      if(page != null)
        query = query.Skip((int)((page - 1) * itemsPerPage)).Take(itemsPerPage);

      return query.ToList();
    }

    public void UpdateVehicle(Vehicle vehicle)
    {
      _connect.Vehicles.Update(vehicle);
      _connect.SaveChanges();
    }
}