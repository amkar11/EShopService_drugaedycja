using System;
using Xunit;
using EShop.Application;
namespace EShop.Application.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ValidateCard_ValidVisaCard_ReturnsTrue()
        {
            // Arrange
            var app = new CreditCardService();
            string validCardNumber = "4111 1111 1111 1111";

            // Act
            var result = app.ValidateCard(validCardNumber);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateCard_InvalidCard_WithLetters_ReturnsFalse()
        {
            // Arrange
            var app = new CreditCardService();
            string invalidCardNumber = "4111 1111 11a1 1111";

            // Act
            var result = app.ValidateCard(invalidCardNumber);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateCard_InvalidCard_FailsLuhnAlgorithm_ReturnsFalse()
        {
            // Arrange
            var app = new CreditCardService();
            string invalidCardNumber = "4111 1111 1111 1112"; // Not valid according to Luhn's algorithm

            // Act
            var result = app.ValidateCard(invalidCardNumber);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateCard_ValidCardWithSpaces_ReturnsTrue()
        {
            // Arrange
            var app = new CreditCardService();
            string validCardNumber = "4111 1111 1111 1111"; // Same as valid, but with spaces

            // Act
            var result = app.ValidateCard(validCardNumber);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetCardType_ValidVisaCard_ReturnsVisa()
        {
            // Arrange
            var app = new CreditCardService();
            string visaCardNumber = "4111 1111 1111 1111";

            // Act
            var result = app.GetCardType(visaCardNumber);

            // Assert
            Assert.Equal("Visa", result);
        }

        [Fact]
        public void GetCardType_ValidMasterCard_ReturnsMasterCard()
        {
            // Arrange
            var app = new CreditCardService();
            string masterCardNumber = "5111 1111 1111 1111";

            // Act
            var result = app.GetCardType(masterCardNumber);

            // Assert
            Assert.Equal("MasterCard", result);
        }

        [Fact]
        public void GetCardType_ValidDiscoverCard_ReturnsDiscover()
        {
            // Arrange
            var app = new CreditCardService();
            string discoverCardNumber = "6011 1111 1111 1111";

            // Act
            var result = app.GetCardType(discoverCardNumber);

            // Assert
            Assert.Equal("Discover", result);
        }

        [Fact]
        public void GetCardType_UnknownCard_ReturnsCardNumber()
        {
            // Arrange
            var app = new CreditCardService();
            string unknownCardNumber = "1234 5678 9101 1121"; // Unrecognized format

            // Act
            var result = app.GetCardType(unknownCardNumber);

            // Assert
            Assert.Equal(unknownCardNumber, result);
        }

    }
}