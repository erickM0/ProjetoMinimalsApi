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

        [TestMethod]
         public void FindByIdTest(){

            //Arrange
            var context = CreateTestContext();
          

            var admService = new AdmService(context);

            var adm = new Adm(){
                
                Email = "teste@teste.com",
                Password = "12345",
                Profile = "Admin",
                Id = 6,
            };
            admService.Create(adm);

            //Action
            var admresult = admService.SearchById(6);

            //Asserts

            Assert.AreEqual(adm,admresult);

        }

        [TestMethod]
         public void SearchAlltest(){

            //Arrange
            var context = CreateTestContext();
          

            var admService = new AdmService(context);

            var adm = new Adm(){
                
                Email = "teste1@teste.com",
                Password = "12345",
                Profile = "Edit",
                
            };
            admService.Create(adm);

            var adm1 = new Adm(){
                
                Email = "teste1@teste.com",
                Password = "12345",
                Profile = "Edit",
                
            };
            admService.Create(adm1);
            var adm2 = new Adm(){
                
                Email = "teste1@teste.com",
                Password = "12345",
                Profile = "Edit",
                
            };
            admService.Create(adm2);

            //Action
            var list  = admService.All(1);

           //Asserts

            Assert.AreEqual(3,list.Count());

        }

        [TestMethod]
         public void DeleteTest(){

            //Arrange
            var context = CreateTestContext();
            var admService = new AdmService(context);
            var adm = new Adm(){
                
                Email = "teste1@teste.com",
                Password = "12345",
                Profile = "Edit",
                Id = 6
            };
            admService.Create(adm);

           
            //Action
            admService.Delete(adm);   
            
            //Asserts
            Assert.AreEqual(null,admService.SearchById(adm.Id));

        }
        
    }
