namespace TechnicalInterview.OmniaRetail.Api.Endpoints
{
    internal static class ApiEndpointsConstants
    {
        private const string ApiBase = "api";


        public static class Product
        {
            private const string Base = $"{ApiBase}/products";
            public const string GetAll = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string GetPrices = $"{Base}/{{id}}/prices";
            public const string GetHighestPrice = $"{GetPrices}/highest";
            public const string GetPriceRecommendations = $"{GetPrices}/recommendations";
            public const string Update = $"{Base}/{{id}}";
            public const string UpdatePrices = $"{Base}/prices";
        }

        public static class ProductGroup
        {
            private const string Base = $"{ApiBase}/products/groups";
            public const string GetAll = Base;
            public const string GetCompetitors = $"{Base}/competitors";

        }
    }
}
