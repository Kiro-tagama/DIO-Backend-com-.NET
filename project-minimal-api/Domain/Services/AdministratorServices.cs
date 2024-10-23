using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interface;
using MinimalApi.DTOs;
using MinimalApi.Infrastructure.Db;

namespace MinimalApi.Domain.Services;

public class AdministratorServices : IAdministratorServices{

    private readonly DbConnect _connect;
    public AdministratorServices(DbConnect connect){
      _connect = connect;
    }

    public Administrator? Login(LoginDTO loginDTO)
    {
      var adm = _connect.Administrators.Where(
        x => x.Email == loginDTO.Email && x.Password == loginDTO.Password).FirstOrDefault();
      
      return adm;
    }
}