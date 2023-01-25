using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarrierPricingClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var getQuotesRequest = new GetQuotesRequest
            {
                Pickup_Postcode = "SW1A1AA",
                Delivery_Postcode = "EC2A3LT",
                Vehicle = "bicycle"
            };

            var json = JsonSerializer.Serialize(getQuotesRequest);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://localhost:44369/api/Pricing/quotes";
            var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
