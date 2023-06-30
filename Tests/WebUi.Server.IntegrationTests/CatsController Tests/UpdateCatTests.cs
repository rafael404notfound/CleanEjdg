
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests
{
    public class UpdateCatTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;
        public UpdateCatTests(IntegrationTestFactory factory)
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
        public async void Returns_Succes_When_Cat_Is_Updated()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(TestCats);
            var requestCat = new Cat()
            {
                Id = 1,
                Name = "Susan",
                DateOfBirth = new DateTime(2019, 6, 6),
                IsSterilized = false,
                IsVaccinated = false,
                HasChip = false,
            };
            var content = JsonContent.Create(requestCat);

            // Act
            var response = await client.PutAsync("api/Cats", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void Returns_404_When_Cat_Is_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(TestCats);
            var requestCat = new Cat()
            {
                Id = 3,
                Name = "Susan",
                DateOfBirth = new DateTime(2019, 6, 6),
                IsSterilized = false,
                IsVaccinated = false,
                HasChip = false,
            };
            var content = JsonContent.Create(requestCat);

            // Act
            var response = await client.PutAsync("api/Cats", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async void Returns_Updated_Cat_When_Cat_Is_Updated()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(TestCats);
            var requestCat = new Cat()
            {
                Id = 1,
                Name = "Susan",
                DateOfBirth = new DateTime(2019, 6, 6),
                IsSterilized = false,
                IsVaccinated = false,
                HasChip = false,
            };
            var content = JsonContent.Create(requestCat);

            // Act
            var response = await client.PutAsync("api/Cats", content);

            // Assert
            var resultResponseCat = await response.Content.ReadFromJsonAsync<Cat>() ?? new Cat();
            Assert.Equal(requestCat.Id, resultResponseCat.Id);
            Assert.Equal(requestCat.Name, resultResponseCat.Name);
            Assert.Equal(requestCat.DateOfBirth, resultResponseCat.DateOfBirth);
            Assert.Equal(requestCat.IsSterilized, resultResponseCat.IsSterilized);
            Assert.Equal(requestCat.IsVaccinated, resultResponseCat.IsVaccinated);
            Assert.Equal(requestCat.HasChip, resultResponseCat.HasChip);
        }

        [Fact]
        public async void Updates_Cat()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(TestCats);
            var requestCat = new Cat()
            {
                Id = 1,
                Name = "Susan",
                DateOfBirth = new DateTime(2019, 6, 6),
                IsSterilized = false,
                IsVaccinated = false,
                HasChip = false,
            };
            var content = JsonContent.Create(requestCat);

            // Act
            var response = await client.PutAsync("api/Cats", content);

            // Assert
            var dbOptions = _factory.GetDbContextOptions();
            var context = new PgsqlDbContext(dbOptions);
            var result = context.Cats.ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(requestCat.Id, (result.Find(c => c.Id == 1) ?? new Cat()).Id);
            Assert.Equal(requestCat.Name, (result.Find(c => c.Id == 1) ?? new Cat()).Name);
            Assert.Equal(requestCat.DateOfBirth, (result.Find(c => c.Id == 1) ?? new Cat()).DateOfBirth);
            Assert.Equal(requestCat.IsSterilized, (result.Find(c => c.Id == 1) ?? new Cat()).IsSterilized);
            Assert.Equal(requestCat.IsVaccinated, (result.Find(c => c.Id == 1) ?? new Cat()).IsVaccinated);
            Assert.Equal(requestCat.HasChip, (result.Find(c => c.Id == 1) ?? new Cat()).HasChip);
        }
    }
}
