using System.Collections.Generic;

namespace CarrierPricingAPI.Model
{
    public class GetQuotesResponse
    {
        public string Pickup_Postcode { get; set; }
        public string Delivery_Postcode { get; set; }
        public string? Vehicle { get; set; }
        public List<ServicePrice> Price_List { get; set; }
    }

    public class ServicePrice
    {
        public string Service { get; set; }
        public long Price { get; set; }
        public int Delivery_Time { get; set; }

    }
}
