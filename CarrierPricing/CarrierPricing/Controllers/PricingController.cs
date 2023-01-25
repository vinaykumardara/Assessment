using CarrierPricing.Service;
using CarrierPricingAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CarrierPricing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly IPricingService _pricingService;
        private readonly ILogger<PricingController> _logger;

        public PricingController(IPricingService pricingService, ILogger<PricingController> logger)
        {
            _pricingService = pricingService;
            _logger = logger;
        }

        /// <summary>
        /// API Health Check
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Pricing API is running");
        }

        /// <summary>
        /// Get price quoting for choosen vehicle
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>price list response</returns>
        [HttpPost("quotes")]
        public async Task<ActionResult<GetQuotesResponse>> Quotes(GetQuotesRequest payload)
        {
            try
            {
                var response = await _pricingService.Quotes(payload)
                    .ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("The following exceptions have occurred: " + ex.Message);
                return null;
            }
        }
    }
}
