namespace Trainline.ExchangeRate.Api.Models.Requests
{
    public class ConvertionRequest
    {
        public double Price { get; set; }
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
    }
}
