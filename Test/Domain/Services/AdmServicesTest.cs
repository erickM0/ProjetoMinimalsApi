using Microsoft.Extensions.Configuration;
using Minimal_Api.Domain.Entities;
using Minimal_Api.Domain.Services;
using Minimal_Api.Infra.DB;
using Test.Infra;

namespace Teste.Domain.Services;

    [TestClass]
    public class AdmServicesTest
    {

        [TestInitialize]
        public void Initialize(){
         var tdb = new TestDatabase();
         tdb.Setup();
        }
        private DbContexto CreateTestContext(){

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json",optional: false,reloadOnChange: true)
            .AddEnvironmentVariables();
            
            var config = builder.Build();
            return new DbContexto(config);
        }
        
        [TestMethod]
        public void SaveAdmTest(){

            //Arrange
            var context = CreateTestContext();
          

            var admService = new AdmService(context);

            var adm = new Adm(){
                Email = "teste@teste.com",
                Password = "12345",
                Profile = "Admin"
            };

            //Action
            admService.Create(adm);

            //Asserts

            Assert.AreEqual(1,admService.All(1).Count());



        }           
    }
