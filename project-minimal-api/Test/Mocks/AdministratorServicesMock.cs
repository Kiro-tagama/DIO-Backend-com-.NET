using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interface;
using MinimalApi.DTOs;

namespace Test.Mocks;

public class AdministratorServicesMock : IAdministratorServices{
    private static List<Administrator> administrators = new List<Administrator>(){
      new Administrator{
        Id = 1,
        Email = "adm@teste.com",
        Password = "123456",
        Profile = "Adm"
      },
      new Administrator{
        Id = 2,
        Email = "editor@teste.com",
        Password = "123456",
        Profile = "Editor"
      }
    };

    public Administrator AddAdministrator(Administrator administrator)
    {
      administrator.Id = administrators.Count() + 1;
      administrators.Add(administrator);

      return administrator;
    }

    public Administrator? GetAdministratorById(int id)
    {
      return administrators.Find(x => x.Id == id);
    }

    public List<Administrator> GetAllAdministrators(int? page)
    {
      return administrators;
    }

    public Administrator? Login(LoginDTO loginDTO)
    {
      return administrators.Find(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);
    }
}