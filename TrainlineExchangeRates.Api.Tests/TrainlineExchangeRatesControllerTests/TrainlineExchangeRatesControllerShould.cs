using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Trainline.ExchangeRate.Api.Models.Requests;
using Trainline.ExchangeRate.Api.Models.Responses;
using Trainline.ExchangeRate.Business.Managers.Interfaces;
using Trainline.ExchangeRate.Business.Managers.Models;
using TrainlineExchangeRates.Api.Controllers;
using Xunit;

namespace TrainlineExchangeRates.Api.Tests.TrainlineExchangeRatesControllerTests
{
    public class TrainlineExchangeRatesControllerShould
    {

        [Fact]
        public async Task ReturnOkObjectWhenRequestIsCorrectAsync()
        {
            // Arrange
            var mockExchangeRatesManager = new Mock<IExchangeRatesManager>();

            // S.U.T. (Subject under test)
            var sut = new TrainlineExchangeRatesController(mockExchangeRatesManager.Object);

            // For 100 GBP, to USD,Given the exchange rate it should return 138.4935 ( 100 * 1.384935 )
            var priceToReturn = 138.4935;
            var fromCurrency = "GBP";
            var toCurrency = "USD";
            mockExchangeRatesManager.Setup(p => p.ConvertFromToCurrencyAsync(100.0, fromCurrency, toCurrency)).ReturnsAsync(new Amount { Price = priceToReturn, Currency = toCurrency });

            //Act
            var response = await sut.GetConvertedAmountAsync(new ConvertionRequest { Price = 100.0, SourceCurrency = fromCurrency, TargetCurrency = toCurrency });

            //Assert
            Assert.IsType<OkObjectResult>(response.Result);
            var result = response.Result as OkObjectResult;

            Assert.Equal(priceToReturn, ((ConvertionResponse)result.Value).Price);
            Assert.Equal(toCurrency, ((ConvertionResponse)result.Value).Currency);
        }


        [Fact]
        public async Task ReturnBadRequestWhenPriceIsLowerThanZeroAsync()
        {
            // Arrange
            var mockExchangeRatesManager = new Mock<IExchangeRatesManager>();

            // S.U.T. (Subject under test)
            var sut = new TrainlineExchangeRatesController(mockExchangeRatesManager.Object);

            var requestPrice = -2;
            var fromCurrency = "GBP";
            var toCurrency = "USD";

            mockExchangeRatesManager.Setup(p => p.ConvertFromToCurrencyAsync(requestPrice, fromCurrency, toCurrency)).ReturnsAsync(It.IsAny<Amount>());

            //Act
            var response = await sut.GetConvertedAmountAsync(new ConvertionRequest { Price = requestPrice, SourceCurrency = fromCurrency, TargetCurrency = toCurrency });

            //Assert
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async Task ReturnBadRequestWhenSourceCurrencyIsMissingAsync()
        {
            // Arrange
            var mockExchangeRatesManager = new Mock<IExchangeRatesManager>();

            // S.U.T. (Subject under test)
            var sut = new TrainlineExchangeRatesController(mockExchangeRatesManager.Object);

            var requestPrice = 100.0;
            var fromCurrency = "";
            var toCurrency = "USD";

            mockExchangeRatesManager.Setup(p => p.ConvertFromToCurrencyAsync(requestPrice, fromCurrency, toCurrency)).ReturnsAsync(It.IsAny<Amount>());

            //Act
            var response = await sut.GetConvertedAmountAsync(new ConvertionRequest { Price = requestPrice, SourceCurrency = fromCurrency, TargetCurrency = toCurrency });

            //Assert
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async Task ReturnBadRequestWhenTargetCurrencyIsMissingAsync()
        {
            // Arrange
            var mockExchangeRatesManager = new Mock<IExchangeRatesManager>();

            // S.U.T. (Subject under test)
            var sut = new TrainlineExchangeRatesController(mockExchangeRatesManager.Object);

            var requestPrice = 100.0;
            var fromCurrency = "GBP";
            var toCurrency = "";

            mockExchangeRatesManager.Setup(p => p.ConvertFromToCurrencyAsync(requestPrice, fromCurrency, toCurrency)).ReturnsAsync(It.IsAny<Amount>());

            //Act
            var response = await sut.GetConvertedAmountAsync(new ConvertionRequest { Price = requestPrice, SourceCurrency = fromCurrency, TargetCurrency = toCurrency });

            //Assert
            Assert.IsType<BadRequestResult>(response.Result);
        }
    }
}
