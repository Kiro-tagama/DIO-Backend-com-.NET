using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;

namespace Test.Domain.Entities;

[TestClass]
public class AdministratorServicesTest
{
    string[] profileType = new string[2] { "Adm", "Editor" };

    private DbConnect CreatingContext(){
        // esse path ref a o arquivo appsettings.json do /Test
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DbConnect(configuration);
    }

    [TestMethod]
    public void TestSaveAdm(){
      var context = CreatingContext();
      context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administrators");

      // Arrange
      var administrator = new Administrator();
      administrator.Id = 1;
      administrator.Email = "john.doe@example.com";
      administrator.Password = "password123";
      administrator.Profile = profileType[0];

      var admService = new AdministratorServices(context);
      
      // act
      admService.AddAdministrator(administrator);

      // assert
      Assert.AreEqual(1, admService.GetAllAdministrators(1).Count());
    }
 
    [TestMethod]
    public void TestFindById(){
      var context = CreatingContext();
      context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administrators");

      // Arrange
      var administrator = new Administrator();
      administrator.Id = 1;
      administrator.Email = "john.doe@example.com";
      administrator.Password = "password123";
      administrator.Profile = profileType[0];

      var admService = new AdministratorServices(context);
      
      // act
      admService.AddAdministrator(administrator);
      var findAdm = admService.GetAdministratorById(administrator.Id);

      // assert
      Assert.AreEqual(1, findAdm?.Id);
    }
}