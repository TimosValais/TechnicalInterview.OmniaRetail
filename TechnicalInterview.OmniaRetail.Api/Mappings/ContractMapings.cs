using TechincalInterview.OmniaRetail.Contracts.Requests;
using TechincalInterview.OmniaRetail.Contracts.Responses;
using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Api.Mappings
{
    public static class ContractMapings
    {
        public static RetailerProductPrice MapToProductRetailerPrice(this UpdateProductPriceRequest request, Guid retailerId)
        {
            return new RetailerProductPrice
            {
                ProductId = request.ProductId,
                RetailerId = retailerId,
                Price = request.Price,
            };
        }

        public static ProductResponse MapToProductResponse(this Product product)
        {
            return new
            (
                Id: product.Id,
                Brand: product.Brand,
                Description: product.Description ?? "No Description",
                Name: product.Name
            );
        }

        public static IEnumerable<ProductResponse> MapToProductResponses(this IEnumerable<Product> products)
        {
            List<ProductResponse> productResponses = [];
            foreach (Product product in products)
            {
                productResponses.Add(MapToProductResponse(product));
            }
            return productResponses;
        }

        //this would normally be an extension on an class named price or something similar
        //I decided not to extend but use parameter
        public static PriceResponse MapToPriceResponse(int price)
        {
            return new(GetPriceStringFromPrice(price));
        }

        public static IEnumerable<PriceResponse> MapToPriceResponses(IEnumerable<int> prices)
        {
            List<PriceResponse> priceResponses = [];
            foreach (int price in prices)
            {
                priceResponses.Add(MapToPriceResponse(price));
            }
            return priceResponses;
        }

        public static ProductPriceResponse MapToProductPriceResponse(this RetailerProductPrice retailerProductPrice)
        {
            return new ProductPriceResponse(retailerProductPrice.ProductId, GetPriceStringFromPrice(retailerProductPrice.Price));
        }
        public static IEnumerable<ProductPriceResponse> MapToProductPriceResponses(this IEnumerable<RetailerProductPrice> retailerProductPrices)
        {
            List<ProductPriceResponse> productPriceResponses = [];
            foreach (RetailerProductPrice price in retailerProductPrices)
            {
                productPriceResponses.Add(price.MapToProductPriceResponse());
            }
            return productPriceResponses;
        }

        public static CompetitorResponse MapToCompetitorResponse(this Retailer retailer)
        {
            return new CompetitorResponse(retailer.Name, retailer.Address);
        }

        public static IEnumerable<CompetitorResponse> MapToCompetitorResponses(this IEnumerable<Retailer> retailers)
        {
            List<CompetitorResponse> competitorResponses = [];
            foreach (Retailer retailer in retailers)
            {
                competitorResponses.Add(retailer.MapToCompetitorResponse());
            }
            return competitorResponses;
        }

        public static ProductGroupResponse MapToProductGroupResponse(this ProductGroup productGroup)
        {
            return new(productGroup.Id, productGroup.Name);
        }

        public static IEnumerable<ProductGroupResponse> MapToProductGroupResponses(this IEnumerable<ProductGroup> productGroups)
        {
            List<ProductGroupResponse> productGroupResponses = [];
            foreach (ProductGroup productGroup in productGroups)
            {
                productGroupResponses.Add(productGroup.MapToProductGroupResponse());
            }
            return productGroupResponses;
        }

        private static string GetPriceStringFromPrice(int price)
        {
            decimal priceDecimal = ((decimal)price) / 100;
            return priceDecimal.ToString("#.00");
        }
    }
}
