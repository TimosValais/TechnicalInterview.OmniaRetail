using FluentAssertions;
using NSubstitute;
using TechincalInterview.OmniaRetail.Contracts.Adapters;
using TechnicalInterview.OmniaRetail.Application.Models;
using TechnicalInterview.OmniaRetail.Application.Persistence.Repositories;
using TechnicalInterview.OmniaRetail.Application.Services;

namespace Application.Tests.Unit
{
    public class ProductServiceTests
    {
        private readonly ProductService _sut;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IRetailerProductPriceRepository _retailerProductPriceRepository = Substitute.For<IRetailerProductPriceRepository>();
        private readonly IProductGroupRepository _productGroupRepository = Substitute.For<IProductGroupRepository>();
        private readonly ILoggerAdapter<ProductService> _logger = Substitute.For<ILoggerAdapter<ProductService>>();

        //public Task<IEnumerable<Product>> ListAsync(CancellationToken cancellationToken = default);
        //public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        //public Task<IEnumerable<int>> GetProductPricesAsync(Guid productId, CancellationToken cancellationToken = default);

        //public Task<int> GetProductHighestTier1PriceAsync(Guid productId, CancellationToken cancellationToken = default);
        //public Task<int> GetPriceRecommendationByIdAsync(Guid productId, PriceTier priceTier = default, CancellationToken cancellationToken = default);

        //public Task<IEnumerable<ProductGroup>> ListProductGroupsAsync(CancellationToken cancelationToken = default!);
        public ProductServiceTests()
        {
            _sut = new ProductService(_productRepository, _retailerProductPriceRepository, _productGroupRepository, _logger);
        }

        [Fact]
        public async Task ListAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            //arrange
            _productRepository.GetAllAsync().Returns(Enumerable.Empty<Product>());


            //act

            IEnumerable<Product> result = await _sut.ListAsync();

            //assert

            result.Should().BeEmpty();
        }


        [Theory]
        [MemberData(nameof(ListOfProductsData))]
        public async Task ListAsync_ShouldReturnProducts_WhenSomeProductsExist(List<Product> expectedProducts)
        {
            //arrange

            _productRepository.GetAllAsync().Returns(expectedProducts);


            //act

            IEnumerable<Product> result = await _sut.ListAsync();

            //assert

            result.Should().BeEquivalentTo(expectedProducts);
        }


        public static IEnumerable<object[]> ListOfProductsData()
        {
            Product product1 = new()
            {
                Id = Guid.NewGuid(),
                Name = "Test1",
                Brand = "Brand1",
                Description = "Test1",
            };
            Product product2 = new()
            {
                Id = Guid.NewGuid(),
                Name = "Test2",
                Brand = "Brand2",
                Description = "Test2",
            };
            Product product3 = new()
            {
                Id = Guid.NewGuid(),
                Name = "Test3",
                Brand = "Brand3",
                Description = "Test3",
            };
            Product product4 = new()
            {
                Id = Guid.NewGuid(),
                Name = "Test4",
                Brand = "Brand4",
                Description = "Test4",
            };

            List<Product> productCollection1 =
            [
                product1,
                product2,
            ];
            List<Product> productCollection2 =
            [
                product3,
                product4,
            ];

            return [
                new object[] { productCollection1},
                new object[] { productCollection2 }
            ];
        }


    }
}
