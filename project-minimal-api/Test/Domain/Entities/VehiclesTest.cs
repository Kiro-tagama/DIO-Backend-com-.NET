// using MinimalApi.Domain.Entities;

// namespace Test.Domain.Entities;

// [TestClass]
// public class VehiclesTest
// {

//     [TestMethod]
//     public void TestMethodGetSetProperties(){
//       var vehicle = new Vehicles();
//       // act
//       vehicle.Id = 1;
//       vehicle.Name = "Camry";
//       vehicle.Mark = "Toyota";
//       vehicle.Year = 2021;

//       // assert
//       Assert.AreEqual(1, vehicle.Id);
//       Assert.AreEqual("Camry", vehicle.Name);
//       Assert.AreEqual("Toyota", vehicle.Mark);
//       Assert.AreEqual(2021, vehicle.Year);

//     }

//     public void TestErrYearVehicle(){
//       var vehicle = new Vehicles();
//       // act
//       vehicle.Year = 1900;

//       // assert
//       Assert.Throws<ArgumentException>(() => vehicle.Year = 1900);
//     }
// }