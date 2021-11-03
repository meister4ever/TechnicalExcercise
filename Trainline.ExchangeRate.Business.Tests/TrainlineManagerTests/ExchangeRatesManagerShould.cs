using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Trainline.ExchangeRate.Api.Models.Responses;
using Trainline.ExchangeRate.Business.Managers;
using Xunit;

namespace Trainline.ExchangeRate.Business.Tests.TrainlineManagerTests
{
    public class ExchangeRatesManagerShould
    {
        [Fact]
        public async Task GetCorrectPriceFromGBPToOtherCurrenciesAsync()
        {
            // Arrange
            // As we exchange from GBP, the table of GBP exchange rates
            // could be similar to something as below
            var testContent = new ExchangeRateResponse
            {
                Rates = new Rates
                {
                    GBP = 1,
                    EUR = 1.168852,
                    USD = 1.384935
                }
            };

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(testContent))
                });

            var underTest = new ExchangeRatesManager(new HttpClient(mockMessageHandler.Object));

            var fromPriceAmount = 100.0;

            // Act
            // Price from GBP to USD
            var gbpToUsdResult = await underTest.ConvertFromToCurrencyAsync(fromPriceAmount, "GBP", "USD");
            // Price from GBP to EUR
            var eurResult = await underTest.ConvertFromToCurrencyAsync(fromPriceAmount, "GBP", "EUR");

            // Assert
            // We should obtain fromPriceAmount * testContent.Rates.USD  
            // 138.4935 on this case ( 100 * 1.384935 )
            // GBP to USD is on this case is 1.384935. The price we should obtain is ( 100 * 1.384935 = 138.4935)
            Assert.Equal(fromPriceAmount * testContent.Rates.USD, gbpToUsdResult.Price);

            // We should obtain fromPriceAmount * testContent.Rates.EUR
            // GBP to EUR on this case is 1.168852. The price we should obtain is ( 100 * 1.168852 = 116.8852)
            Assert.Equal(fromPriceAmount * testContent.Rates.EUR, eurResult.Price);
        }
    }
}
