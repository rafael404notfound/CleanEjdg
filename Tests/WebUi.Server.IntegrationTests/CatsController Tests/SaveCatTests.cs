
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests
{
    public class SaveCatTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _factory;
        public SaveCatTests(IntegrationTestFactory factory)
        {
            _factory = factory;
        }

        private readonly IEnumerable<Cat> TestCats = new List<Cat>()
        {
            new Cat
            {
                Name = "Susan",
                DateOfBirth = new DateTime(2021, 2, 23),
                HasChip = true,
                IsSterilized = true,
                IsVaccinated = true
            },
            new Cat
            {
                Name = "Yuki",
                DateOfBirth = new DateTime(2022, 8, 15),
                HasChip = true,
                IsSterilized = true,
                IsVaccinated = false
            }
        };

        [Fact]
        public async void Saves_Cat()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);
            var requestCat = new CatBindingTarget()
            {
                Name = "TestCat",
                DateOfBirth = new DateTime(2019, 6, 6),
                IsSterilized = true,
                IsVaccinated = true,
                HasChip = true,
            };
            var content = JsonContent.Create(requestCat);

            // Act

            var response = await client.PostAsync("api/Cats", content);

            // Assert           
            var dbOptions = _factory.GetDbContextOptions();
            var context = new PgsqlDbContext(dbOptions);
            var result = context.Cats.ToList();

            Assert.True(result.Count() == 3);
            Assert.Equal("TestCat", result.Last().Name);
        }

        [Fact]
        public async void Returns_Saved_Cat()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(TestCats);
            CatBindingTarget requestCat = new CatBindingTarget()
            {
                Name = "TestCat",
                DateOfBirth = new DateTime(2019, 6, 6),
                IsSterilized = true,
                IsVaccinated = true,
                HasChip = true,
            };
            var content = JsonContent.Create(requestCat);

            // Act
            var response = await client.PostAsync("api/Cats", content);

            // Assert           
            var dbOptions = _factory.GetDbContextOptions();
            var context = new PgsqlDbContext(dbOptions);
            var SavedCat = context.Cats.ToList().Last();
            Cat resultResponseCat = await response.Content.ReadFromJsonAsync<Cat>() ?? new Cat();

            Assert.Equal(SavedCat.Name, resultResponseCat.Name);
            Assert.Equal(SavedCat.Id, resultResponseCat.Id);
            Assert.Equal(SavedCat.IsVaccinated, resultResponseCat.IsVaccinated);
            Assert.Equal(SavedCat.IsSterilized, resultResponseCat.IsSterilized);
            Assert.Equal(SavedCat.HasChip, resultResponseCat.HasChip);
            Assert.Equal(SavedCat.DateOfBirth, resultResponseCat.DateOfBirth);
        }

        [Fact]
        public async void Returns_Succes_When_Cat_Is_Saved()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(TestCats);
            CatBindingTarget requestCat = new CatBindingTarget()
            {
                Name = "TestCat",
                DateOfBirth = new DateTime(2019, 6, 6),
                IsSterilized = true,
                IsVaccinated = true,
                HasChip = true,
            };
            var content = JsonContent.Create(requestCat);

            // Act
            var response = await client.PostAsync("api/Cats", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void Returns_BadRequest_When_Cat_Is_Not_Saved()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(TestCats);
            CatBindingTarget requestCat = new CatBindingTarget()
            {
                Name = "TestCat"
            };
            var content = JsonContent.Create(requestCat);

            // Act
            var response = await client.PostAsync("api/Cats", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void Does_Not_Save_Cat_When_Model_Is_Not_Valid()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.SetDbInitialState(TestCats);
            CatBindingTarget requestCat = new CatBindingTarget()
            {
                Name = "TestCat"
            };
            var content = JsonContent.Create(requestCat);

            // Act
            var response = await client.PostAsync("api/Cats", content);

            // Assert
            var dbOptions = _factory.GetDbContextOptions();
            var context = new PgsqlDbContext(dbOptions);
            var result = context.Cats.ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Susan", result.First().Name);
            Assert.Equal("Yuki", result.Last().Name);

        }
    }
}
