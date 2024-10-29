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

      // Arrange
      var administrator = new Administrator();
      administrator.Id = 1;
      administrator.Email = "john.doe@example.com";
      administrator.Password = "password123";
      administrator.Profile = profileType[0];

      var context = CreatingContext();
      var admService = new AdministratorServices(context);
      
      // act
      admService.

      // assert
      Assert.AreEqual(1, administrator.Id);
      Assert.AreEqual("john.doe@example.com", administrator.Email);
      Assert.AreEqual("password123", administrator.Password);
      Assert.AreEqual(profileType[0], administrator.Profile);
    }
}