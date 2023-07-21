using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests.ProductControllerTests
{
    public class GetProductTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;
        public GetProductTests(IntegrationTestFactory factory)
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
        public async void Returns_Succes_When_Product_Is_Returned()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);

            // Act
            var response = await client.GetAsync("api/products/1");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void Returns_Product()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);

            // Act
            var response = await client.GetAsync("api/products/1");

            // Assert
            Product result = await response.Content.ReadFromJsonAsync<Product>() ?? new Product();
            Assert.Equal("Bolso", result.Name);
        }

        [Fact]
        public async void Returns_404NotFound_When_Product_Is_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);

            // Act
            var response = await client.GetAsync("api/products/3");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
