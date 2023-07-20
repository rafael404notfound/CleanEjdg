
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests
{
    public class GetCatsTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;
        public GetCatsTests(IntegrationTestFactory factory)
        {
            _factory = factory;
        }

        private readonly IEnumerable<Cat> TestCats = new List<Cat>()
        {
            new Cat
            {
                Name = "Susan",
                DateOfBirth = DateTime.SpecifyKind( new DateTime(2021, 2, 23), DateTimeKind.Utc),
                HasChip = true,
                IsSterilized = true,
                IsVaccinated = true,
            },
            new Cat
            {
                Name = "Yuki",
                DateOfBirth =  DateTime.SpecifyKind( new DateTime(2022, 8, 15), DateTimeKind.Utc),
                HasChip = true,
                IsSterilized = true,
                IsVaccinated = false
            }
        };

        [Fact]
        public async void Returns_Success_When_Cats_Are_Returned()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.GetAsync("api/Cats");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void Returns_Cats()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.GetAsync("api/Cats");

            // Assert
            Cat[] result = await response.Content.ReadFromJsonAsync<Cat[]>() ?? new Cat[0];
            Assert.Equal(2, result.Length);
            Assert.Equal("Susan", result[0].Name);
            Assert.Equal("Yuki", result[1].Name);
        }

        [Fact]
        public async void Returns_404NotFound_When_Cats_Are_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(Enumerable.Empty<Cat>());

            // Act
            var response = await client.GetAsync("api/Cats");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
