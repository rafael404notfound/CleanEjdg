using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests.ProductControllerTests
{
    public class SaveProductTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;
        public SaveProductTests(IntegrationTestFactory factory)
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
        public async void Saves_Product()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);
            Product requestProduct = new Product()
            {
                Name = "TestProduct",
                Price = 5,
                Category = "TestCategory",
                Description = "A product specific for testing purposes"
            };
            var content = JsonContent.Create(requestProduct);

            // Act

            var response = await client.PostAsync("api/products", content);

            // Assert           
            var dbOptions = _factory.GetDbContextOptions<ProductDbContext>();
            var context = new ProductDbContext(dbOptions);
            var result = context.Products.Include(c => c.Photos).ToList();

            Assert.True(result.Count() == 3);
            Assert.Equal("TestProduct", result.Last().Name);
        }

        [Fact]
        public async void Returns_Saved_Product()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);
            Product requestProduct = new Product()
            {
                Name = "TestProduct",
                Price = 5,
                Category = "TestCategory",
                Description = "A product specific for testing purposes"
            };
            var content = JsonContent.Create(requestProduct);

            // Act
            var response = await client.PostAsync("api/products", content);

            // Assert           
            var dbOptions = _factory.GetDbContextOptions<ProductDbContext>();
            var context = new ProductDbContext(dbOptions);
            var SavedProduct = context.Products.ToList().Last();
            Product resultResponseProduct = await response.Content.ReadFromJsonAsync<Product>() ?? new Product();

            Assert.Equal(SavedProduct.Name, resultResponseProduct.Name);
            Assert.Equal(SavedProduct.Price, resultResponseProduct.Price);
            Assert.Equal(SavedProduct.Description, resultResponseProduct.Description);
            Assert.Equal(SavedProduct.Category, resultResponseProduct.Category);
        }

        [Fact]
        public async void Returns_Succes_When_Product_Is_Saved()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);
            Product requestProduct = new Product()
            {
                Name = "TestProduct",
                Price = 5,
                Category = "TestCategory",
                Description = "A product specific for testing purposes"
            };
            var content = JsonContent.Create(requestProduct);

            // Act
            var response = await client.PostAsync("api/products", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void Returns_BadRequest_When_Product_Is_Not_Saved()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);
            Product requestProduct = new Product()
            {
                Name = "TestProduct"
            };
            var content = JsonContent.Create(requestProduct);

            // Act
            var response = await client.PostAsync("api/products", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void Does_Not_Save_Product_When_Model_Is_Not_Valid()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(products: TestProducts);
            Product requestProduct = new Product()
            {
                Name = "TestCat"
            };
            var content = JsonContent.Create(requestProduct);

            // Act
            var response = await client.PostAsync("api/products", content);

            // Assert
            var dbOptions = _factory.GetDbContextOptions<ProductDbContext>();
            var context = new ProductDbContext(dbOptions);
            var result = context.Products.ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Bolso", result.First().Name);
            Assert.Equal("Chapa", result.Last().Name);

        }
    }
}
