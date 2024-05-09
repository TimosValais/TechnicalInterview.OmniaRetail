using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Persistence
{
    public class DbInitializer : IDbInitializer
    {

        private readonly AppDbContext _dbContext;

        public DbInitializer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            //if true then db was created, if false it existed
            if (await _dbContext.Database.EnsureCreatedAsync())
            {
                await this.SeedDataAsync();
            };
        }

        private async Task SeedDataAsync()
        {
            List<Product> products =
            [
                new Product { Id = Guid.NewGuid(), Name = "Laptop", Description = "High-performance laptop.", Brand = "Brand A" },
                new Product { Id = Guid.NewGuid(), Name = "Smartphone", Description = "Latest model smartphone.", Brand = "Brand B" },
                new Product { Id = Guid.NewGuid(), Name = "Tablet", Description = "Portable and powerful tablet.", Brand = "Brand C" },
                new Product { Id = Guid.NewGuid(), Name = "Monitor", Description = "4K HDR monitor.", Brand = "Brand D" },
                new Product { Id = Guid.NewGuid(), Name = "Keyboard", Description = "Mechanical gaming keyboard.", Brand = "Brand E" },
                new Product { Id = Guid.NewGuid(), Name = "Mouse", Description = "Wireless ergonomic mouse.", Brand = "Brand F" }
            ];

            List<Retailer> retailers =
            [
                new Retailer { Id = new Guid("e3a87e20-36d3-42ee-8993-c8dfbfa01c3b"), Name = "Retailer 1", Address = "1234 North Street" },
                new Retailer { Id = new Guid("bc024a7e-c4f6-42a5-a67d-76b8c9efd432"), Name = "Retailer 2", Address = "5678 South Street" },
                new Retailer { Id = new Guid("6e94c46e-a0b1-4831-bea9-fa48900c3065"), Name = "Retailer 3", Address = "91011 East Boulevard" },
                new Retailer { Id = new Guid("3a66173d-ff1f-4458-9bd9-2fc27887ad35"), Name = "Retailer 4", Address = "121314 West Avenue" }
            ];

            List<ProductGroup> productGroups =
            [
                new ProductGroup { Id = Guid.NewGuid(), Name = "Electronics" },
                new ProductGroup { Id = Guid.NewGuid(), Name = "Accessories" },
                new ProductGroup { Id = Guid.NewGuid(), Name = "Office Supplies" }
            ];

            // Assigning products to groups
            productGroups[0].Products.Add(products[0]); // Laptop to Electronics
            productGroups[0].Products.Add(products[1]); // Smartphone to Electronics
            productGroups[0].Products.Add(products[2]); // Tablet to Electronics

            productGroups[1].Products.Add(products[3]); // Monitor to Accessories
            productGroups[1].Products.Add(products[4]); // Keyboard to Accessories
            productGroups[1].Products.Add(products[5]); // Mouse to Accessories

            // All products also to Office Supplies for overlap
            productGroups[2].Products.AddRange(products);

            // Set up prices for each product at each retailer
            List<RetailerProductPrice> prices = [];
            foreach (Product product in products)
            {
                foreach (Retailer retailer in retailers)
                {
                    prices.Add(new RetailerProductPrice
                    {
                        ProductId = product.Id,
                        RetailerId = retailer.Id,
                        //Product = product,
                        //Retailer = retailer,
                        Price = new Random().Next(5000, 20000)  // Random price for simplicity
                    });
                }
            }


            await _dbContext.Products.AddRangeAsync(products);
            await _dbContext.ProductGroups.AddRangeAsync(productGroups);
            await _dbContext.Retailers.AddRangeAsync(retailers);
            await _dbContext.RetailerProductPrices.AddRangeAsync(prices);

            await _dbContext.SaveChangesAsync();
        }

    }
}
