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

    public Administrator AddAdministrator(Administrator administrator)
    {
      _connect.Administrators.Add(administrator);
      _connect.SaveChanges();

      return administrator;
    }

    public Administrator? GetAdministratorById(int id){
      return _connect.Administrators.Where(x => x.Id == id).FirstOrDefault();
    }

    public List<Administrator> GetAllAdministrators(int? page)
    {
      var query = _connect.Administrators.AsQueryable();

      int itemsPerPage = 10;

      if(page != null)
        query = query.Skip((int)((page - 1) * itemsPerPage)).Take(itemsPerPage);

      return query.ToList();
    }

    public Administrator? Login(LoginDTO loginDTO){
      var adm = _connect.Administrators.Where(
        x => x.Email == loginDTO.Email && x.Password == loginDTO.Password).FirstOrDefault();
      
      return adm;
    }
}