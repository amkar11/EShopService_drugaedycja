using Xunit;
using Moq;
using EShop.Application;
using EShop.Domain;
using EShopService_drugaedycja.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EShopService_drugaedycja.Controllers
{

    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IRedisCacheService> _redisCacheServiceMock;
        private readonly ProductController _controller;
        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _cacheServiceMock = new Mock<ICacheService>();
            _redisCacheServiceMock = new Mock<IRedisCacheService>();
            _controller = new ProductController(_productServiceMock.Object, _cacheServiceMock.Object, _redisCacheServiceMock.Object);
        }
        [Fact]
        public async Task ShowAllProducts_ReturnsOkResult_WithListOfProducts()
        {
            DateTime dateTime = DateTime.Now;
            IEnumerable<Product> products = new List<Product>
        {
            new Product {Id = 0, Name = "Phone", Price = 25.56m, Created_at = dateTime, Uploaded_at = dateTime},
            new Product {Id = 1, Name = "Computer", Price = 4000, Created_at = dateTime, Uploaded_at = dateTime},
        };
            _redisCacheServiceMock.Setup(service => service.GetOrAddValueAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IEnumerable<Product>>>>(),
                It.IsAny<TimeSpan>())).ReturnsAsync(products);
           
            var result = await _controller.ShowAllProductsAsync();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal(2, returnProducts.Count());
        }

        [Fact]
        public async Task GetProductById_ReturnsOkResult_WithProduct()
        {
            DateTime dateTime = DateTime.Now;
            Product product = new Product { Id = 0, Name = "Car", Price = 25000, Created_at = dateTime, Uploaded_at = dateTime };
            _redisCacheServiceMock.Setup(service => service.GetOrAddValueAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<Product>>>(),
                It.IsAny<TimeSpan>())).ReturnsAsync(product);

            var result = await _controller.GetProductByIdAsync(0);
            var okResult = Assert.IsType<OkObjectResult>(result);

            var productResult = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(product.Id, productResult.Id);
            Assert.Equal(product.Name, productResult.Name);
            Assert.Equal(product.Price, productResult.Price);
            Assert.Equal(product.Created_at, productResult.Created_at);
            Assert.Equal(product.Uploaded_at, productResult.Uploaded_at);
        }

        [Fact]
        public async Task AddProduct_ReturnsCreatedResult_WithProductPathAndProductItself()
        {
            DateTime dateTime = DateTime.Now;
            Product product = new Product { Id = 0, Name = "Car", Price = 25000, Created_at = dateTime, Uploaded_at = dateTime };
            _productServiceMock.Setup(service => service.AddProductAsync(product));
            var result = await _controller.AddProductAsync(product);
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal($"api/Product/{product.Id}", createdResult?.Location);
            var returnedProduct = Assert.IsType<Product>(createdResult?.Value);
            Assert.Equal(product.Name, returnedProduct.Name);
            Assert.Equal(product.Price, returnedProduct.Price);
        }

        [Fact]
        public async Task ChangeProduct_ReturnsNoContent()
        {
            // Arrange
            var dateTime = DateTime.Now;
            var product = new Product
            {
                Id = 1,
                Name = "Updated Car",
                Price = 30000,
                Created_at = dateTime,
                Uploaded_at = dateTime
            };

            _productServiceMock.Setup(service => service.UpdateProductAsync(product));

            // Act
            var result = await _controller.ChangeProductAsync(1, product);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _productServiceMock.Verify(service => service.UpdateProductAsync(product), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            // Arrange
            var dateTime = DateTime.Now;
            var product = new Product
            {
                Id = 1,
                Name = "Updated Car",
                Price = 30000,
                Created_at = dateTime,
                Uploaded_at = dateTime
            };
            _productServiceMock.Setup(service => service.AddProductAsync(product));
            _productServiceMock.Setup(service => service.DeleteProductAsync(1));

            // Act
            var result = await _controller.DeleteProductAsync(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _productServiceMock.Verify(service => service.DeleteProductAsync(1), Times.Once);
        }
    }
}