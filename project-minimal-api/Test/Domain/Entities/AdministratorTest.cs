using MinimalApi.Domain.Entities;

namespace Test.Domain.Entities;

[TestClass]
public class AdministratorTest
{
    string[] profileType = new string[2] { "Adm", "Editor" };

    [TestMethod]
    public void TestMethodGetSetProperties(){

      // Arrange
      var administrator = new Administrator();

      // act
      administrator.Id = 1;
      administrator.Email = "john.doe@example.com";
      administrator.Password = "password123";
      administrator.Profile = profileType[0];

      // assert
      Assert.AreEqual(1, administrator.Id);
      Assert.AreEqual("john.doe@example.com", administrator.Email);
      Assert.AreEqual("password123", administrator.Password);
      Assert.AreEqual(profileType[0], administrator.Profile);
    }
}