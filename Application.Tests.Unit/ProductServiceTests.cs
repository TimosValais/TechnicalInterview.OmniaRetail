using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TechincalInterview.OmniaRetail.Contracts;
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

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNoProductExists()
        {
            //arrange

            _productRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

            //act
            Product? result = await _sut.GetByIdAsync(Guid.NewGuid());

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            //arrange

            Product existingProduct = new()
            {
                Id = Guid.NewGuid(),
                Brand = "Brand",
                Name = "Name",
                Description = "Description"
            };

            _productRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(existingProduct);

            //act
            Product? result = await _sut.GetByIdAsync(existingProduct.Id);

            //assert
            result.Should().BeEquivalentTo(existingProduct);
        }

        [Fact]
        public async Task GetProductPricesAsync_ShouldReturnEmpty_WhenNoPricesExist()
        {
            //arrange
            _retailerProductPriceRepository.GetAllByProductIdAsync(Arg.Any<Guid>()).Returns(Enumerable.Empty<RetailerProductPrice>());
            //act
            IEnumerable<int> result = await _sut.GetProductPricesAsync(Guid.NewGuid());
            //assert
            result.Should().BeEmpty();

        }

        [Fact]
        public async Task GetProductPricesAsync_ShouldReturnCorrectPrices_WhenPricesExist()
        {
            //arrange
            Guid productId = Guid.NewGuid();
            Guid retailerId = Guid.NewGuid();
            List<RetailerProductPrice> retailerProductPrices =
            [
                new()
                {
                    Price = 1000,
                    ProductId = productId,
                    RetailerId = retailerId
                },
                new()
                {
                    Price = 2000,
                    ProductId = productId,
                    RetailerId = retailerId
                },
                new()
                {
                    Price = 3000,
                    ProductId = productId,
                    RetailerId = retailerId
                },
            ];
            _retailerProductPriceRepository.GetAllByProductIdAsync(productId).Returns(retailerProductPrices);
            //act
            IEnumerable<int> result = await _sut.GetProductPricesAsync(productId);
            //assert
            result.Should().BeEquivalentTo(retailerProductPrices.Select(r => r.Price));

        }
        [Fact]
        public async Task GetProductHighestTier1PriceAsync_ShouldReturnZero_WhenNoPricesExist()
        {
            //arrange
            _retailerProductPriceRepository.GetAllByProductIdAsync(Arg.Any<Guid>()).Returns(Enumerable.Empty<RetailerProductPrice>());
            //act
            int result = await _sut.GetProductHighestTier1PriceAsync(Guid.NewGuid());
            //assert
            result.Equals(0);
        }

        [Theory]
        [MemberData(nameof(ListOfPricesAndRetailProductPrices))]
        public async Task GetProductHighestTier1PriceAsync_ShouldReturnHighestPrice_WhenPricesExist(List<int> prices, List<RetailerProductPrice> retailerProductPrices)
        {
            //arrange
            Guid productId = Guid.NewGuid();
            _retailerProductPriceRepository.GetAllByProductIdAsync(productId).Returns(retailerProductPrices);
            //act
            int result = await _sut.GetProductHighestTier1PriceAsync(productId);
            //assert
            result.Equals(prices.Max());
        }

        [Fact]
        public async Task GetPriceRecommendationByIdAsync_ShouldThrowError_WhenNoRecommendationIsFound()
        {
            //arrange
            _retailerProductPriceRepository.GetAllByProductIdAsync(Arg.Any<Guid>()).Returns(Enumerable.Empty<RetailerProductPrice>());
            //act
            Func<Task<int>> action = async () => await _sut.GetPriceRecommendationByIdAsync(Guid.NewGuid());
            //assert
            await action.Should().ThrowAsync<InvalidDataException>().WithMessage("No prices where found to recommend");
        }

        [Theory]
        [MemberData(nameof(GetPriceRecommendations))]
        public async Task GetPriceRecommendationByIdAsync_ShouldGiveTheCorrectRecommendation_WhenPricesExist(PriceTier priceTier, List<RetailerProductPrice> retailerProductPrices, int expectedPrice)
        {
            //arrange
            Guid productId = Guid.NewGuid();
            _retailerProductPriceRepository.GetAllByProductIdAsync(productId).Returns(retailerProductPrices);
            //act
            int result = await _sut.GetPriceRecommendationByIdAsync(productId, priceTier);
            //assert
            result.Should().Be(expectedPrice);
        }

        [Fact]
        public async Task ListProductGroupsAsync_ShouldReturnEmptyList_WhenNoProductGroupsExist()
        {
            //arrange
            _productGroupRepository.GetAllAsync().Returns(Enumerable.Empty<ProductGroup>());


            //act

            IEnumerable<ProductGroup> result = await _sut.ListProductGroupsAsync();

            //assert

            result.Should().BeEmpty();
        }


        [Fact]
        public async Task ListProductGroupsAsync_ShouldReturnProducts_WhenSomeProductGroupsExist()
        {
            //arrange
            List<ProductGroup> expectedProductGroups = [
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 1",
                },
                              new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name 2",
                }
            ];
            _productGroupRepository.GetAllAsync().Returns(expectedProductGroups);


            //act

            IEnumerable<ProductGroup> result = await _sut.ListProductGroupsAsync();

            //assert

            result.Should().BeEquivalentTo(expectedProductGroups);
        }

        #region Member Data Methods
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
        public static IEnumerable<object[]> ListOfPricesAndRetailProductPrices()
        {
            List<int> priceCollection1 =
            [
                1000,
                2000,
                3000,
                4000,
                5000,
                6000,
                7000
            ];
            List<int> priceCollection2 =
            [
                100000,
                200000,
                300000,
                400000,
                500000,
                600000,
                700000
            ];

            List<RetailerProductPrice> retailProductPrices1 = [];
            List<RetailerProductPrice> retailProductPrices2 = [];
            foreach (int price in priceCollection1)
            {
                retailProductPrices1.Add(new()
                {
                    Price = price,
                    ProductId = Guid.NewGuid(),
                    RetailerId = Guid.NewGuid(),
                });
            }
            foreach (int price in priceCollection2)
            {
                retailProductPrices2.Add(new()
                {
                    Price = price,
                    ProductId = Guid.NewGuid(),
                    RetailerId = Guid.NewGuid(),
                });
            }

            return [
                new object[] { priceCollection1, retailProductPrices1},
                new object[] { priceCollection2, retailProductPrices2}
            ];
        }

        public static IEnumerable<object[]> GetPriceRecommendations()
        {
            Guid productId = Guid.NewGuid();
            Guid retailerId = Guid.NewGuid();

            #region tier 1 3 cases
            PriceTier tier1 = PriceTier.Tier1;
            //case 1 (good case)
            List<RetailerProductPrice> pricesTier1Case1 = [];
            int expectedPriceTier1Case1 = 3000;
            pricesTier1Case1.Add(new()
            {
                Price = 1000,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case1.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case1.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case1.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case1.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case1.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case1.Add(new()
            {
                Price = 3001,
                ProductId = productId,
                RetailerId = retailerId,
            });

            //case 2 (no tier 1 prices, but tier 2 exist)
            List<RetailerProductPrice> pricesTier1Case2 = [];
            int expectedPriceTier1Case2 = 1002;
            pricesTier1Case2.Add(new()
            {
                Price = 1000,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case2.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case2.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case2.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case2.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case2.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier1Case2.Add(new()
            {
                Price = 100,
                ProductId = productId,
                RetailerId = retailerId,
            });

            //case 3 (no tier 1 prices, no tier 2 prices, only tier 3)
            //TODO: find out if and how is this possible, I think it is not mathematically
            #endregion
            #region tier 2 3 cases
            PriceTier tier2 = PriceTier.Tier2;
            //case 1 (good case)
            List<RetailerProductPrice> pricesTier2Case1 = [];
            int expectedPriceTier2Case1 = 998;
            pricesTier2Case1.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier2Case1.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier2Case1.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier2Case1.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier2Case1.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier2Case1.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            //case 2 no tier 2 prices but tier 3 prices
            //TODO: find out if and how is this possible

            #endregion
            #region tier 3 3 cases
            PriceTier tier3 = PriceTier.Tier3;
            //case 1 (good case)
            List<RetailerProductPrice> pricesTier3Case1 = [];
            int expectedPriceTier3Case1 = 49;
            pricesTier3Case1.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier3Case1.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier3Case1.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier3Case1.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier3Case1.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier3Case1.Add(new()
            {
                Price = 50,
                ProductId = productId,
                RetailerId = retailerId,
            });
            //case 2 (no tier 3 prices but tier 2 exist)
            List<RetailerProductPrice> pricesTier3Case2 = [];
            int expectedPriceTier3Case2 = 998;
            pricesTier3Case2.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier3Case2.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier3Case2.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier3Case2.Add(new()
            {
                Price = 1001,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier3Case2.Add(new()
            {
                Price = 999,
                ProductId = productId,
                RetailerId = retailerId,
            });
            pricesTier3Case2.Add(new()
            {
                Price = 5000,
                ProductId = productId,
                RetailerId = retailerId,
            });
            //case 3 (no tier 3 prices, no tier 2 prices, only tier 1)
            //TODO: find out if and how is this possible, I think it is not mathematically
            #endregion

            return [
                new object[] { tier1, pricesTier1Case1, expectedPriceTier1Case1},
                new object[] { tier1, pricesTier1Case2, expectedPriceTier1Case2},
                new object[] { tier2, pricesTier2Case1, expectedPriceTier2Case1},
                new object[] { tier3, pricesTier3Case1, expectedPriceTier3Case1},
                new object[] { tier3, pricesTier3Case2, expectedPriceTier3Case2},
            ];
        }

        #endregion


    }
}
