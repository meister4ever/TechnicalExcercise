using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Trainline.ExchangeRate.Api.Models.Responses;
using Trainline.ExchangeRate.Business.Managers.Interfaces;
using Trainline.ExchangeRate.Business.Managers.Models;

namespace Trainline.ExchangeRate.Business.Managers
{
    public class ExchangeRatesManager : IExchangeRatesManager
    {
        public HttpClient HttpClient
        {
            get;
            set;
        }

        public ExchangeRatesManager(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        /// <summary>
        /// Gets the resulting Price and the target currency (to which that resulting price was converted)
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="sourceCurrency">Source Currency</param>
        /// <param name="targetCurrency">Target Currency</param>
        /// <returns>Converted Price and Currency</returns>
        public async Task<Amount> ConvertFromToCurrencyAsync(double fromPrice, string sourceCurrency, string targetCurrency)
        {
            var host = "http://trainlinerecruitment.github.io";
            var route = "/exchangerates/api/latest/";

            // We make the request to get the latest exchange rate for the target currency
            var response = await this.HttpClient.GetAsync(host + route + sourceCurrency + ".json");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var exchangeRateResponse = JsonConvert.DeserializeObject<ExchangeRateResponse>(responseBody);

            double currentExchangeRateToTargetCurrency = 0.0;

            // We check based on the target currency
            switch (targetCurrency)
            {
                case "EUR":
                    currentExchangeRateToTargetCurrency = exchangeRateResponse.Rates.EUR;
                    break;

                case "GBP":
                    currentExchangeRateToTargetCurrency = exchangeRateResponse.Rates.GBP;
                    break;

                case "USD":
                    currentExchangeRateToTargetCurrency = exchangeRateResponse.Rates.USD;
                    break;
            }

            // We multiply the price from the current exchange rate in order to get the price in the target currency
            return new Amount { Price = fromPrice * currentExchangeRateToTargetCurrency, Currency = targetCurrency };
        }
    }
}
