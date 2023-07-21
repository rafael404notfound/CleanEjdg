using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests.ProductControllerTests
{
    public class DeleteProductTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;
        public DeleteProductTests(IntegrationTestFactory factory)
        {
            _factory = factory;
        }

        private readonly IEnumerable<Product> TestProducts = new List<Product>()
        {
            new Product
            {
                Id = 1,
                Name = "Bolso",
                Price = 10,
                Description = "Un bolso negro muy chulo",
                Category = "Prenda Unisex"
            },
            new Product
            {
                Id = 2,
                Name = "Chapa",
                Price = 1,
                Description = "Chapas de colores de gatos",
                Category = "Objeto"
            },
        };

        [Fact]
        public async void Returns_Succes_When_Product_Is_Deleted()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);

            // Act
            var response = await client.DeleteAsync("api/products/1");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void Deletes_Product()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);

            // Act
            var response = await client.DeleteAsync("api/products/1");

            // Assert
            var dbOptions = _factory.GetDbContextOptions<ProductDbContext>();
            var context = new ProductDbContext(dbOptions);
            var result = context.Products.ToList();

            Assert.True(result.Count() == 1);
            Assert.Equal("Chapa", result.First().Name);
        }

        [Fact]
        public async void Returns_404_When_Product_Is_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);

            // Act
            var response = await client.DeleteAsync("api/products/3");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

