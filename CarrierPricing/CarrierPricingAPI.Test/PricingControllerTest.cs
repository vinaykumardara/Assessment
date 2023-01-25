using CarrierPricing.Controllers;
using CarrierPricing.Service;
using CarrierPricingAPI.Model;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Xunit;

namespace CarrierPricingAPI.Test
{
    public class PricingControllerTest
    {
        [Fact]
        public void GetQuotes_Success()
        {
            //Arrange
            var dummyRequest = new GetQuotesRequest { Delivery_Postcode = "SW1A1AA", Pickup_Postcode = "EC2A3LT" };
            var dummpyQuote = new GetQuotesResponse
            {
                Delivery_Postcode = "SW1A1AA",
                Pickup_Postcode = "EC2A3LT",
                Vehicle = "",
                Price_List = new List<ServicePrice> {
                    new ServicePrice {
                        Service = "CollectTimes",
                        Price = 326,
                        Delivery_Time = 1
                    },
                    new ServicePrice {
                        Service = "CollectTimes",
                        Price = 321,
                        Delivery_Time = 3
                    },
                    new ServicePrice {
                        Service = "CollectTimes",
                        Price = 319,
                        Delivery_Time = 5
                    },
                    new ServicePrice {
                        Service = "RoyalPackages",
                        Price = 318,
                        Delivery_Time = 3
                    },
                    new ServicePrice {
                        Service = "RoyalPackages",
                        Price = 331,
                        Delivery_Time = 1
                    },
                    new ServicePrice {
                        Service = "Hercules",
                        Price = 319,
                        Delivery_Time = 5
                    },
                    new ServicePrice {
                        Service = "Hercules",
                        Price = 316,
                        Delivery_Time = 10
                    }
                }
            };

            var pricingService = A.Fake<IPricingService>();
            var logger = A.Fake<ILogger<PricingController>>();
            A.CallTo(() => pricingService.Quotes(dummyRequest)).Returns(dummpyQuote);
            var controller = new PricingController(pricingService, logger);

            //Act
            var actionResult = controller.Quotes(dummyRequest);

            //Assert
            var result = actionResult.Result.Result as OkObjectResult;
            var returnQuote = result.Value as GetQuotesResponse;

            Assert.Equal(dummpyQuote, returnQuote);
            Assert.NotNull(returnQuote);
            Assert.NotNull(returnQuote.Price_List);
        }

        [Fact]
        public void GetQuotes_Null()
        {
            //Arrange
            var dummyRequest = new GetQuotesRequest { Delivery_Postcode = "", Pickup_Postcode = "" };
            var dummpyQuote = new GetQuotesResponse { };
            var pricingService = A.Fake<IPricingService>();
            var logger = A.Fake<ILogger<PricingController>>();
            A.CallTo(() => pricingService.Quotes(dummyRequest)).Returns(dummpyQuote);
            var controller = new PricingController(pricingService, logger);

            //Act
            var actionResult = controller.Quotes(dummyRequest);

            //Assert
            var result = actionResult.Result.Result as OkObjectResult;
            var returnQuote = result.Value as GetQuotesResponse;
            Assert.Equal(dummpyQuote, returnQuote);
        }

        [Fact]
        public void GetQuotes_Error()
        {
            //Arrange
            var dummyRequest = new GetQuotesRequest { Delivery_Postcode = "1D...", Pickup_Postcode = "AD..D" };
            var dummpyQuote = new GetQuotesResponse { };
            var pricingService = A.Fake<IPricingService>();
            var logger = A.Fake<ILogger<PricingController>>();
            A.CallTo(() => pricingService.Quotes(dummyRequest)).Returns(dummpyQuote);
            var controller = new PricingController(pricingService, logger);

            //Act
            var actionResult = controller.Quotes(dummyRequest);

            //Assert
            Assert.ThrowsAsync<Exception>(() => actionResult);
        }
    }
}
