using MinimalApi.DTOs;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interface;

public interface IAdministratorServices{
  Administrator? Login(LoginDTO loginDTO);

  Administrator AddAdministrator(Administrator administrator);

  List<Administrator> GetAllAdministrators(int? page);

  Administrator? GetAdministratorById(int id);
}