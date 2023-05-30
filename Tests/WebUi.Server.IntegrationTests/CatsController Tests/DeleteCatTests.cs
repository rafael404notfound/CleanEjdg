
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests
{
    public class DeleteCatTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;
        public DeleteCatTests(IntegrationTestFactory factory)
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
        public async void Returns_Succes_When_Cat_Is_Deleted()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.DeleteAsync("api/Cats/1");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void Deletes_Cat()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.DeleteAsync("api/Cats/1");

            // Assert
            var dbOptions = _factory.GetDbContextOptions();
            var context = new PgsqlDbContext(dbOptions);
            var result = context.Cats.ToList();

            Assert.True(result.Count() == 1);
            Assert.Equal("Yuki", result.First().Name);
        }

        [Fact]
        public async void Returns_404_When_Cat_Is_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.DeleteAsync("api/Cats/3");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
