using System;

namespace Trainline.ExchangeRate.Api.Models.Responses
{
    public class Rates
    {
        public double GBP { get; set; }
        public double EUR { get; set; }
        public double USD { get; set; }
    }

    public class ExchangeRateResponse
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public int Time_last_updated { get; set; }
        public Rates Rates { get; set; }
    }
}
