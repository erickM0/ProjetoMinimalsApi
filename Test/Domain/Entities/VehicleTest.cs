
using Minimal_Api.Domain.Entities;

namespace Teste.Domain.Entities;

    [TestClass]
    public class VehicleTest
    {
        [TestMethod]
        public void GetSetPropertiesTest()
        {
            //Arrange
            var vehicle = new Vehicle(){
                //Act
                Id = 1,
                Name = "Uno",
                Brand = "Fiat",
                Year = 2002           
            };

           //Assert
            Assert.AreEqual(1,vehicle.Id);
            Assert.AreEqual("Uno",vehicle.Name);
            Assert.AreEqual("Fiat",vehicle.Brand);
            Assert.AreEqual(2002,vehicle.Year);

        }
    }
