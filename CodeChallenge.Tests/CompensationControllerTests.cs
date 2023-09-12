
using System;
using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var compensation = new Compensation()
            {
                EmployeeId = "62c1084e-6e34-4630-93fd-9153afb65309",
                Salary = 112.05m,
                EffectiveDate = DateTime.Now

            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var createdCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(compensation.EmployeeId);
            Assert.AreEqual(compensation.EmployeeId, createdCompensation.EmployeeId);
            Assert.AreEqual(compensation.Salary, createdCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, createdCompensation.EffectiveDate);
        }

        [TestMethod]
        public void GetCompensationByEmployeeId_Returns_Ok()
        {
            // Arrange
            string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            decimal expectedSalary = 115000;
            var expectedEffectiveDate = new DateTime(2021, 09, 20, 00, 00, 00);

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensationResult = response.DeserializeContent<Compensation>();
            Assert.AreEqual(expectedSalary, compensationResult.Salary);
            Assert.AreEqual(expectedEffectiveDate, compensationResult.EffectiveDate);
        }

        [TestMethod]
        public void GetCompensationByEmployeeId_Returns_NotFound()
        {
            // Arrange
            var compensation = new Compensation()
            {
                EmployeeId = "Invalid_Id",
                Salary = 20m,
                EffectiveDate = DateTime.Now
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{compensation.EmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}