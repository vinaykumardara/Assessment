using System.Collections.Generic;

namespace CarrierPricing.Data
{
    public class Constants
    {
        public const long LongNumber = 100000000;
        public const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Min = "-1Y2P0IJ32E8E8";
        public const string Max = "1Y2P0IJ32E8E7";

        public static readonly Dictionary<string, int> VehiclePricingRate = new Dictionary<string, int> 
        {
            {"bicycle", 10 },
            {"motorbike", 20 },
            {"parcel_car", 30 },
            {"small_van", 40 },
            {"large_van", 50 },
        };
    }
}
