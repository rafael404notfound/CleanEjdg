
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests.ProductControllerTests
{
    public class GetProductsTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;
        public GetProductsTests(IntegrationTestFactory factory)
        {
            _factory = factory;
        }

        private readonly IEnumerable<Product> TestProducts = new List<Product>()
        {
            new Product
            {
                Name = "Bolso",
                Price = 10,
                Description = "Un bolso negro muy chulo",
                Category = "Prenda Unisex"
            },
            new Product
            {
                Name = "Chapa",
                Price = 1,
                Description = "Chapas de colores de gatos",
                Category = "Objeto"
            },
        };

        [Fact]
        public async void Returns_Success_When_Products_Are_Returned()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);

            // Act
            var response = await client.GetAsync("api/products");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void Returns_Cats()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);

            // Act
            var response = await client.GetAsync("api/products");

            // Assert
            Product[] result = await response.Content.ReadFromJsonAsync<Product[]>() ?? new Product[0];
            Assert.Equal(2, result.Length);
            Assert.Equal("Bolso", result[0].Name);
            Assert.Equal("Chapa", result[1].Name);
        }

        [Fact]
        public async void Returns_404NotFound_When_Cats_Are_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: Enumerable.Empty<Product>());

            // Act
            var response = await client.GetAsync("api/Products");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
