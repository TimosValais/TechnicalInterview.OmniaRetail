using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TechincalInterview.OmniaRetail.Contracts.Adapters;
using TechnicalInterview.OmniaRetail.Application.Models;
using TechnicalInterview.OmniaRetail.Application.Persistence.Repositories;
using TechnicalInterview.OmniaRetail.Application.Services;

namespace Application.Tests.Unit
{
    public class RetailServiceTests
    {
        private readonly RetailerService _sut;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IRetailerProductPriceRepository _retailerProductPriceRepository = Substitute.For<IRetailerProductPriceRepository>();
        private readonly ILoggerAdapter<RetailerService> _logger = Substitute.For<ILoggerAdapter<RetailerService>>();


        public RetailServiceTests()
        {
            _sut = new RetailerService(_productRepository, _retailerProductPriceRepository, _logger);
        }





        [Fact]
        public async Task GetCompetitorsByProductGroupIdAsync_ShouldReturnEmptyList_WhenNoCompetitorsExist()
        {
            //arrange

            _productRepository.GetProductsByGroupIdAsync(Arg.Any<Guid>(), Arg.Any<bool>()).Returns(Enumerable.Empty<Product>());

            //act

            IEnumerable<Retailer> result = await _sut.GetCompetitorsByProductGroupIdAsync(Guid.NewGuid(), Guid.NewGuid());
            //assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCompetitorsByProductGroupIdAsync_ShouldReturnCorrectCompetitors_WhenCompetitorsExist()
        {
            //arrange
            Guid groupId = Guid.NewGuid();
            Guid retailerId = Guid.NewGuid();
            Guid competitor1Id = Guid.NewGuid();
            Guid competitor2Id = Guid.NewGuid();
            Retailer retailer = new()
            {
                Id = retailerId,
                Name = "TestRetailer",
            };
            Retailer competitor1 = new()
            {
                Id = competitor1Id,
                Name = "Test",
            };
            Retailer competitor2 = new()
            {
                Id = competitor2Id,
                Name = "Test2",
            };
            Product product = new()
            {
                Id = Guid.NewGuid(),
                Brand = "Brand",
                Name = "Name",

            };

            product.RetailerProductPrices.Add(new()
            {
                Retailer = competitor1,
                RetailerId = competitor1Id,
                Product = new() { Id = product.Id },
                ProductId = product.Id
            });
            product.RetailerProductPrices.Add(new()
            {
                Retailer = competitor2,
                RetailerId = competitor2Id,
                Product = new() { Id = product.Id },
                ProductId = product.Id
            });
            _productRepository.GetProductsByGroupIdAsync(groupId, true).Returns(new List<Product>()
            {
                product
            });

            //act

            IEnumerable<Retailer> result = await _sut.GetCompetitorsByProductGroupIdAsync(groupId, retailerId);
            //assert
            result.Should().Contain(competitor1);
            result.Should().Contain(competitor2);
            result.Should().NotContain(retailer);
        }

        [Fact]
        public async Task UpdatePricesAsync_ShouldReturnTrue_WhenAnUpdateSucceeded()
        {
            //arrange

            List<RetailerProductPrice> productRetailerPrices =
            [
                new()
                {
                    ProductId = Guid.NewGuid(),
                    RetailerId = Guid.NewGuid(),
                }
            ];
            _retailerProductPriceRepository.UpdateBatchAsync(Arg.Any<IEnumerable<RetailerProductPrice>>()).Returns(true);
            //act
            bool result = await _sut.UpdatePricesAsync(productRetailerPrices);
            //assert
            result.Should().Be(true);
        }


        [Fact]
        public async Task UpdatePricesAsync_ShouldReturnFalse_WhenNoUpdateSucceeded()
        {
            //arrange

            List<RetailerProductPrice> productRetailerPrices =
            [
                new()
                {
                    ProductId = Guid.NewGuid(),
                    RetailerId = Guid.NewGuid(),
                }
            ];
            _retailerProductPriceRepository.UpdateBatchAsync(Arg.Any<IEnumerable<RetailerProductPrice>>()).Returns(false);
            //act
            bool result = await _sut.UpdatePricesAsync(productRetailerPrices);
            //assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task UpdatePricesAsync_ShouldReturnFalse_WhenAnExceptionIsThrown()
        {
            //arrange
            string exceptionMessage = "Doesn't Matter";
            Exception exception = new(exceptionMessage);
            List<RetailerProductPrice> productRetailerPrices =
            [
                new()
                {
                    ProductId = Guid.NewGuid(),
                    RetailerId = Guid.NewGuid(),
                }
            ];
            _retailerProductPriceRepository.UpdateBatchAsync(Arg.Any<IEnumerable<RetailerProductPrice>>()).Throws(exception);
            //act
            bool result = await _sut.UpdatePricesAsync(productRetailerPrices);
            //assert
            _logger.Received(1).LogError(Arg.Is(exception), Arg.Is(exceptionMessage));
            result.Should().Be(false);
        }

        //public async Task<bool> UpdatePricesAsync(IEnumerable<RetailerProductPrice> productRetailerPrices, CancellationToken cancellationToken = default!)
        //{
        //    try
        //    {
        //        return await _retailerProductPriceRepository.UpdateBatchAsync(productRetailerPrices, cancellationToken);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return false;
        //    }
        //}
    }
}
