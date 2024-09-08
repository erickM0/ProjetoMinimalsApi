using Microsoft.Extensions.Configuration;
using Minimal_Api.Domain.Entities;
using Minimal_Api.Domain.Services;
using Minimal_Api.Infra.DB;
using Test.Infra;

namespace Teste.Domain.Services;

    [TestClass]
    public class VehicleServicesTest
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
        public void SaveVehicleTest(){

            //Arrange
            var context = CreateTestContext();
          

            var vehicleService = new VehicleService(context);

            var vehicle = new Vehicle(){
                
                Name = "Fiat",
                Brand = "Palio",
                Year = 1999
            };

            //Action
            vehicleService.Include(vehicle);

            //Asserts
            Assert.AreEqual(1,vehicleService.All(1).Count());

        }       

        [TestMethod]
         public void FindByIdTest(){

            //Arrange
            var context = CreateTestContext();
          

            var vehicleService = new VehicleService(context);

            var vehicle = new Vehicle(){
                
                Id = 6,
                Name = "Ka",
                Brand = "Ford",
                Year = 2000,
            };
            vehicleService.Include(vehicle);

            //Action
            var result = vehicleService.SearchById(6);

            //Asserts

            Assert.AreEqual(vehicle,result);

        }

        [TestMethod]
         public void SearchAlltest(){

            //Arrange
            var context = CreateTestContext();
          

            var vehicleService = new VehicleService(context);

            var vehicle = new Vehicle(){
                
                Name = "Ranger",
                Brand = "Ford",
                Year = 2012
                
            };
            vehicleService.Include(vehicle);

            var vehicle1 = new Vehicle(){
                
                Name = "Camaro",
                Brand = "Fiat",
                Year = 2020
                
            };
            vehicleService.Include(vehicle1);

            var vehicle2 = new Vehicle(){
                
                Name = "Soul",
                Brand = "Kia",
                Year = 2021,
            };
            vehicleService.Include(vehicle2);

            //Action
            var list  = vehicleService.All(1);

           //Asserts

            Assert.AreEqual(3,list.Count());

        }

        [TestMethod]
         public void DeleteTest(){

            //Arrange
            var context = CreateTestContext();
            var vehicleServiceService = new VehicleService(context);
            var vehicleService = new VehicleService(context);

            var vehicle = new Vehicle(){
                
                Name = "Ranger",
                Brand = "Ford",
                Year = 2012,
                Id = 1
                
            };
            vehicleService.Include(vehicle);

            var vehicle1 = new Vehicle(){
                
                Name = "Camaro",
                Brand = "Fiat",
                Year = 2020,
                Id = 2, 
                
            };
            vehicleService.Include(vehicle1);

            var vehicle2 = new Vehicle(){
                
                Name = "Soul",
                Brand = "Kia",
                Year = 2021,
                Id = 3
            };
            vehicleService.Include(vehicle2);

           
            //Action
            vehicleService.Delete(vehicle);   
            
            //Asserts
            Assert.AreEqual(null,vehicleService.SearchById(vehicle.Id));
            Assert.AreEqual(vehicle1,vehicleService.SearchById(vehicle1.Id));


        }
        
    }
