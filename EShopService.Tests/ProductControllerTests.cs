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
        private readonly ProductController _controller;
        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductController(_productServiceMock.Object);
        }
        [Fact]
        public void ShowAllProducts_ReturnsOkResult_WithListOfProducts()
        {
            var products = new List<Product>
        {
            new Product {Name = "Phone", Price = 25.56m},
            new Product {Name = "Computer", Price = 4000},
        };
            _productServiceMock.Setup(service => service.ShowAllProductsAsync()).Returns(products);
            var result =  _controller.ShowAllProductsAsync();
            var OkResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(OkResult.Value);
            Assert.Equal(2, ((List<Product>)returnProducts).Count);
        }

        [Fact]
        public void GetProductById_ReturnsOkResult_WithProduct()
        {
            DateTime dateTime = DateTime.Now;
            Product product = new Product { Id = 0, Name = "Car", Price = 25000, Created_at = dateTime, Uploaded_at = dateTime};
            _productServiceMock.Setup(service => service.GetProductByIdAsync(0)).Returns(product);
            var result = _controller.GetProductByIdAsync(0);
            var OkResult = Assert.IsType<OkObjectResult>(result);

            var productResult = Assert.IsType<Product>(OkResult.Value);
            Assert.Equal(product.Id, productResult.Id);
            Assert.Equal(product.Name, productResult.Name);
            Assert.Equal(product.Price, productResult.Price);
            Assert.Equal(product.Created_at, productResult.Created_at);
            Assert.Equal(product.Uploaded_at, productResult.Uploaded_at);
        }

        [Fact]
        public void AddProduct_ReturnsCreatedResult_WithProductPathAndProductItself()
        {
            DateTime dateTime = DateTime.Now;
            Product product = new Product { Id = 0, Name = "Car", Price = 25000, Created_at = dateTime, Uploaded_at = dateTime };
            _productServiceMock.Setup(service => service.AddProduct(product));
            var result = _controller.AddProduct(product);
            var createdResult = Assert.IsType<CreatedResult>(result); 
            Assert.Equal($"api/Product/{product.Id}", createdResult?.Location); 
            var returnedProduct = Assert.IsType<Product>(createdResult?.Value);
            Assert.Equal(product.Name, returnedProduct.Name); 
            Assert.Equal(product.Price, returnedProduct.Price);
        }

        [Fact]
        public void ChangeProduct_ReturnsNoContent()
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

            _productServiceMock.Setup(service => service.UpdateProduct(product));

            // Act
            var result = _controller.ChangeProduct(1, product);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _productServiceMock.Verify(service => service.UpdateProduct(product), Times.Once);
        }

        [Fact]
        public void DeleteProduct_ReturnsNoContent()
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
            _productServiceMock.Setup(service => service.AddProduct(product));
            _productServiceMock.Setup(service => service.DeleteProduct(1));

            // Act
            var result = _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _productServiceMock.Verify(service => service.DeleteProduct(1), Times.Once);
        }
    }
}