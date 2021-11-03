using System.Threading.Tasks;
using Trainline.ExchangeRate.Business.Managers.Models;

namespace Trainline.ExchangeRate.Business.Managers.Interfaces
{
    public interface IExchangeRatesManager
    {
        public Task<Amount> ConvertFromToCurrencyAsync(double price, string sourceCurrency, string targetCurrency);
    }
}
