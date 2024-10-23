using MinimalApi.DTOs;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interface;

public interface IAdministratorServices{
  Administrator? Login(LoginDTO loginDTO);
}