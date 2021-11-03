using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Trainline.ExchangeRate.Api.Models.Requests;
using Trainline.ExchangeRate.Api.Models.Responses;
using Trainline.ExchangeRate.Business.Managers.Interfaces;

namespace TrainlineExchangeRates.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TrainlineExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRatesManager ExchangeRatesManager;

        public TrainlineExchangeRatesController(IExchangeRatesManager exchangeRatesManager)
        {
            ExchangeRatesManager = exchangeRatesManager;
        }

        /// <summary>
        /// Receives a price, source currency and target currency.
        /// Gets the price converted to the target currency and target currency.
        /// </summary>
        /// <param name="request">Price, source currency and target currency</param>
        /// <response code="200">Successful Listing</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("ConvertAmount")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConvertionResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConvertionResponse>> GetConvertedAmountAsync(
            [FromQuery] ConvertionRequest request)
        {
            // Validate empty or invalid currencies
            if (   request.Price < 0 ||
                   request.SourceCurrency == null || request.SourceCurrency == "" ||
                   request.TargetCurrency == null || request.SourceCurrency == "" ||
                   !IsValid(request.SourceCurrency) || 
                   !IsValid(request.TargetCurrency))
                return BadRequest();

            // Convert the price from source currency to the target currency using the manager for exchange rates
            var convertedAmount = await ExchangeRatesManager.ConvertFromToCurrencyAsync(request.Price, 
                                                                                        request.SourceCurrency, 
                                                                                        request.TargetCurrency);

            // We return the price converted to the target currency and target currency
            return Ok(new ConvertionResponse { Price = convertedAmount.Price, 
                                            Currency = convertedAmount.Currency });
        }

        private static bool IsValid(string currency)
        {
            return currency == "EUR" || currency == "GBP" || currency == "USD";
        }
    }
}
