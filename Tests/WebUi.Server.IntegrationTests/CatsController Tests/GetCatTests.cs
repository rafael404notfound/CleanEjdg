
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests
{
    public class GetCatTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;
        public GetCatTests(IntegrationTestFactory factory)
        {
            _factory = factory;
        }

        private readonly IEnumerable<Cat> TestCats = new List<Cat>()
        {
            new Cat
            {
                Id = 1,
                Name = "Susan",
                DateOfBirth = new DateTime(2021, 2, 23),
                HasChip = true,
                IsSterilized = true,
                IsVaccinated = true
            },
            new Cat
            {
                Id = 2,
                Name = "Yuki",
                DateOfBirth = new DateTime(2022, 8, 15),
                HasChip = true,
                IsSterilized = true,
                IsVaccinated = false
            }
        };

        [Fact]
        public async void Returns_Succes_When_Cat_Is_Returned()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.GetAsync("api/Cats/1");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void Returns_Cat()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.GetAsync("api/Cats/1");

            // Assert
            Cat result = await response.Content.ReadFromJsonAsync<Cat>() ?? new Cat();
            Assert.Equal("Susan", result.Name);
        }

        [Fact]
        public async void Returns_404NotFound_When_Cat_Is_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.GetAsync("api/Cats/3");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
