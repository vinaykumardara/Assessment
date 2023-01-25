using CarrierPricingAPI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CarrierPricing.Data.Constants;

namespace CarrierPricing.Service
{
    public class PricingService : IPricingService
    {
        public async Task<GetQuotesResponse> Quotes(GetQuotesRequest getQuotesRequest)
        {
            decimal price = 0;

            if (getQuotesRequest?.Pickup_Postcode?.Length > 0 && getQuotesRequest?.Delivery_Postcode?.Length > 0)
            {
                long startPoint = CommonService.Decode(getQuotesRequest.Pickup_Postcode);
                long endPoint = CommonService.Decode(getQuotesRequest.Delivery_Postcode);

                price = Math.Abs(startPoint - endPoint) / LongNumber;
            }

            if (getQuotesRequest?.Vehicle?.Length > 0 && VehiclePricingRate.ContainsKey(getQuotesRequest.Vehicle))
            {
                var percentage = VehiclePricingRate[getQuotesRequest.Vehicle];
                decimal result = Math.Round(price * percentage / 100);
                price = price + result;
            }

            var carries = await GetCarriersAsync(getQuotesRequest).ConfigureAwait(false);
            var priceList = await GetPriceListAsync(carries, price).ConfigureAwait(false);

            GetQuotesResponse getQuotesResponse = new GetQuotesResponse()
            {
                Pickup_Postcode = getQuotesRequest.Pickup_Postcode,
                Delivery_Postcode = getQuotesRequest.Delivery_Postcode,
                Vehicle = getQuotesRequest.Vehicle,
                Price_List = priceList
            };

            return await Task.FromResult(getQuotesResponse);
        }

        private async Task<List<ServicePrice>> GetPriceListAsync(List<Carriers> carriers, decimal quote_price)
        {
            List<ServicePrice> price_list = new List<ServicePrice>();

            foreach (var item in carriers.Where(x => x.base_price > 0))
            {
                if (item != null)
                {
                    var base_price = item.base_price;
                    var carrier_name = item.carrier_name;
                    if (item.services != null)
                    {
                        foreach (var service in item.services)
                        {
                            var markup = service.markup;
                            var price = quote_price + CommonService.GetPercentageValue(base_price, markup);
                            price_list.Add(new ServicePrice
                            {
                                Service = carrier_name,
                                Price = (long)price,
                                Delivery_Time = service.delivery_time
                            });
                        }
                    }
                }
            }

            return await Task.FromResult(price_list).ConfigureAwait(false);
        }

        private async Task<List<Carriers>> GetCarriersAsync(GetQuotesRequest getQuotesRequest)
        {
            string vehicle = getQuotesRequest.Vehicle;
            string filePath = Path.Combine(AppContext.BaseDirectory + @"/Data/carrier-data.json");
            List<Carriers> carriers = new List<Carriers>();

            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                carriers = JsonConvert.DeserializeObject<List<Carriers>>(json);
            }

            var result = carriers?.Where(x => x.services != null).ToList();

            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item.services != null)
                    {
                        List<CarrierPricingAPI.Model.Service> services = new List<CarrierPricingAPI.Model.Service>();

                        foreach (var service in item.services)
                        {
                            if (service.vehicles != null)
                            {
                                foreach (var vehiclegrp in service.vehicles)
                                {
                                    if (vehiclegrp.Equals(getQuotesRequest.Vehicle))
                                        services.Add(service);
                                }
                            }
                        }

                        if (services.Count > 0)
                        {
                            carriers.Add(new Carriers
                            {
                                base_price = item.base_price,
                                carrier_name = item.carrier_name,
                                services = services.ToList()
                            });
                        }
                    }
                }
            }

            return await Task.FromResult(carriers).ConfigureAwait(false);
        }
    }

    public interface IPricingService
    {
        public Task<GetQuotesResponse> Quotes(GetQuotesRequest getQuotesRequest);
    }
}
