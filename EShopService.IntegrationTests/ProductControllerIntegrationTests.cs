using System.Net;
using System.Net.Http;
using EShop.Domain;
using EShopService_drugaedycja;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using Xunit;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EShopService.IntegrationTests
{
    [TestCaseOrderer("EShopService.IntegrationTests.TestOrdered", "EShopService.IntegrationTests")]
    [Collection("NoParallel")]
    public class ProductControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public ProductControllerIntegrationTests(CustomWebApplicationFactory factory) {
            _client = factory.CreateClient();

        }
        
        [Fact, TestPriority(1)]
        public async Task ProductController_CheckHttpGet_ReturnOK()
        {
            var response = await _client.GetAsync("/api/Product");
            var get = response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(2)]
        public async Task ProductController_CheckHttpGetId_ReturnOk()
        {
            
            var response = await _client.GetAsync($"/api/Product/{1}");
            var get = response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact, TestPriority(3)]
        public async Task ProductController_CheckHttpPost_ReturnCreated()
        {
            Product product = new Product { Id = 4, Name = "Sanya", Ean = "43t79rh8fney78dj", Price = 50, Stock = 250, Sku = "ABC4545", Created_by = Guid.NewGuid(), Updated_by = Guid.NewGuid() };
            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Product", content);
            var errorJson = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }

        [Fact, TestPriority(4)]
        public async Task ProductController_CheckHttpPut_ReturnNoContent()
        {
            Product product = new Product { Id = 1, Name = "Sanya", Ean = "43t79rh8fney78dj", Price = 50, Stock = 250, Sku = "ABC4545", Created_by = Guid.NewGuid(), Updated_by = Guid.NewGuid() };
            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response_get = await _client.GetAsync($"/api/Product/{1}");
            var first_product = await response_get.Content.ReadAsStringAsync();

            var response = await _client.PutAsync($"/api/Product/{1}", content);

            var response_get_after = await _client.GetAsync($"/api/Product/{1}");
            var second_product = await response_get_after.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }

        [Fact, TestPriority(5)]
        public async Task ProductController_CheckHttpDelete_ReturnNoContent()
        {
            var response = await _client.GetAsync($"/api/Product/{1}");
            var check_product = await response.Content.ReadAsStringAsync();

            var delete = await _client.DeleteAsync($"/api/Product/{1}");

            var response_deleted = await _client.GetAsync($"/api/Product/{1}");
            var check_product_deleted = await response_deleted.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NoContent , delete.StatusCode);
        }

        [Fact, TestPriority(6)]
        public async Task ProductController_CheckHttpPatch_ReturnNoContent()
        {
            var response = await _client.GetAsync($"/api/Product/{2}");
            var check_product = await response.Content.ReadAsStringAsync();

            var patchDoc = new JsonPatchDocument<ProductDTO>();
            patchDoc.Replace(p => p.Name, "Adrian");
            patchDoc.Replace(p => p.Price, 300);
            patchDoc.Replace(p => p.Stock, 300);
            var json = JsonConvert.SerializeObject(patchDoc);
            var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");

            var patch = await _client.PatchAsync($"api/Product/{2}", content);

            var response_patch = await _client.GetAsync($"/api/Product/{2}");
            var check_patch = await response_patch.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NoContent , patch.StatusCode);
        }

    }
}