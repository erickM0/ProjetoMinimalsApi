using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Minimal_Api.Domain.DTOs;
using Minimal_Api.Domain.ModelsView;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
    public class AdmRequestsTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Setup.ClassInit(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup(){
            Setup.ClassCleanup();
        }

        [TestMethod]
        public async Task TestGetSetPropertie(){
            // Arrange
            var loginDTO  = new LoginDTO{
                Email = "teste@teste",
                Password = "teste123"
            };

            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");


            // Act
            var response = await  Setup.client.PostAsync("/admin/login", content);   

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result =  await response.Content.ReadAsStringAsync();
            var admLogged = JsonSerializer.Deserialize<LoggedAdmModelView>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            Assert.IsNotNull(admLogged?.Profile);
            Assert.IsNotNull(admLogged?.Email);
            Assert.IsNotNull(admLogged?.Token);
        }
        
    }
}