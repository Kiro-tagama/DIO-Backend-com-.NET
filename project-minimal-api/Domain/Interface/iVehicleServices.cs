using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interface;

public interface IVehicleServices{
  List<Vehicle> GetVehicles(int page = 1, string? name = null, string? mark = null);
  Vehicle? GetVehicleById(int id);
  void AddVehicle(Vehicle vehicle);
  void UpdateVehicle(Vehicle vehicle);
  void DeleteVehicle(Vehicle vehicle);
}