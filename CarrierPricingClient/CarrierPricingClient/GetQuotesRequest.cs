namespace CarrierPricingClient
{
    public class GetQuotesRequest
    {
        public string Pickup_Postcode { get; set; }
        public string Delivery_Postcode { get; set; }
        public string Vehicle { get; set; }
    }
}
