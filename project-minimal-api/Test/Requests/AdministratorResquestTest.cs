using System.Net;
using System.Text;
using System.Text.Json;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.ModelViews;
using MinimalApi.DTOs;
using Test.Helpes;

namespace Test.Resquests;

[TestClass]
public class AdministratorRequestTest
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext context){
      Setup.ClassInit(context);
    }

    [ClassCleanup]
    public static void ClassCleanup(){
      Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task TestMethodAddAdministratorAsync(){
      var loginDTO = new LoginDTO{
        Email = "adm@teste.com",
        Password = "123456"
      };

      var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8,  "Application/json");

      // Act
      var response = await Setup.client.PostAsync("/administrator/login", content);

      // Assert
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

      var result = await response.Content.ReadAsStringAsync();
      var admLoggedIn = JsonSerializer.Deserialize<AdmLoggedIn>(result, new JsonSerializerOptions
      {
          PropertyNameCaseInsensitive = true
      });

      Assert.IsNotNull(admLoggedIn?.Email ?? "");
      Assert.IsNotNull(admLoggedIn?.Profile ?? "");
      Assert.IsNotNull(admLoggedIn?.Token ?? "");

      Console.WriteLine(admLoggedIn?.Token);
    }
}