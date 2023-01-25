using System.Collections.Generic;

namespace CarrierPricingAPI.Model
{
    public class Carriers
    {
        public string carrier_name { get; set; }
        public int base_price { get; set; }
        public List<Service> services { get; set; }
    }

    public class Service
    {
        public int delivery_time { get; set; }
        public int markup { get; set; }
        public List<string> vehicles { get; set; }
    }


}
