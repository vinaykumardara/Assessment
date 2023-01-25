using System.ComponentModel.DataAnnotations;

namespace CarrierPricingAPI.Model
{
    public class GetQuotesRequest
    {
        [Required]
        public string Pickup_Postcode { get; set; }
        [Required]
        public string Delivery_Postcode { get; set; }
        public string? Vehicle { get; set; }
    }
}
