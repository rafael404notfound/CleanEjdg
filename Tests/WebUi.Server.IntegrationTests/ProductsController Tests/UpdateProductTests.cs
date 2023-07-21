
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests.ProductControllerTests
{
    public class UpdateProductTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;
        public UpdateProductTests(IntegrationTestFactory factory)
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
        public async void Returns_Succes_When_Product_Is_Updated()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);
            var requestProduct = new Product
            {
                Id = 1,
                Name = "Bolso",
                Price = 20,
                Description = "Un bolso negro muy chulo",
                Category = "Prenda Unisex"
            };
            var content = JsonContent.Create(requestProduct);

            // Act
            var response = await client.PutAsync("api/products", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void Returns_404_When_Product_Is_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);
            var requestProduct = new Product
            {
                Id = 3,
                Name = "Bolso",
                Price = 20,
                Description = "Un bolso negro muy chulo",
                Category = "Prenda Unisex"
            };
            var content = JsonContent.Create(requestProduct);

            // Act
            var response = await client.PutAsync("api/products", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async void Returns_Updated_Cat_When_Product_Is_Updated()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);
            var requestProduct = new Product
            {
                Id = 1,
                Name = "Bolso",
                Price = 20,
                Description = "Un bolso negro muy chulo",
                Category = "Prenda Unisex"
            };
            var content = JsonContent.Create(requestProduct);

            // Act
            var response = await client.PutAsync("api/products", content);

            // Assert
            var resultResponseProduct = await response.Content.ReadFromJsonAsync<Product>() ?? new Product();
            Assert.Equal(requestProduct.Id, resultResponseProduct.Id);
            Assert.Equal(requestProduct.Name, resultResponseProduct.Name);
            Assert.Equal(requestProduct.Price, resultResponseProduct.Price);
            Assert.Equal(requestProduct.Description, resultResponseProduct.Description);
            Assert.Equal(requestProduct.Category, resultResponseProduct.Category);
        }

        [Fact]
        public async void Updates_Product()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);
            var requestProduct = new Product
            {
                Id = 1,
                Name = "Bolso",
                Price = 20,
                Description = "Un bolso negro muy chulo",
                Category = "Prenda Unisex"
            };
            var content = JsonContent.Create(requestProduct);

            // Act
            var response = await client.PutAsync("api/products", content);

            // Assert
            var dbOptions = _factory.GetDbContextOptions<ProductDbContext>();
            var context = new ProductDbContext(dbOptions);
            var result = context.Products.ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(requestProduct.Id, (result.Find(c => c.Id == 1) ?? new Product()).Id);
            Assert.Equal(requestProduct.Name, (result.Find(c => c.Id == 1) ?? new Product()).Name);
            Assert.Equal(requestProduct.Price, (result.Find(c => c.Id == 1) ?? new Product()).Price);
            Assert.Equal(requestProduct.Description, (result.Find(c => c.Id == 1) ?? new Product()).Description);
            Assert.Equal(requestProduct.Category, (result.Find(c => c.Id == 1) ?? new Product()).Category);
        }
    }
}
