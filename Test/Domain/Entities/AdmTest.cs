
using Minimal_Api.Domain.Entities;

namespace Teste.Domain.Entities;

    [TestClass]
    public class AdmTest
    {
        [TestMethod]
        public void GetSetPropertiesTest()
        {
            //Arrange
            var adm = new Adm(){
                //Act
                Id = 1,
                Email = "teste@teste.com",
                Profile = "Admin",
                Password = "Senha123"
                

            };

           //Assert
            Assert.AreEqual(1,adm.Id);
            Assert.AreEqual("teste@teste.com",adm.Email);
            Assert.AreEqual("Admin",adm.Profile);
            Assert.AreEqual("Senha123",adm.Password);
        }
    }
