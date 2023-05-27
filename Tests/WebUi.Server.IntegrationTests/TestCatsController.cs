
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests {
    
    public class TestCatsController : IClassFixture<IntegrationTestFactory> 
    {
        private readonly IntegrationTestFactory _factory;

        public TestCatsController(IntegrationTestFactory factory)
        {
            _factory = factory;
        }

        // HttpGet GetCats() Tests:

        [Fact]
        public async void HttpGet_GetCats_Endpoint_Returns_Success_When_Db_Is_Not_Emtpy()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.RestoreInitialState();

            // Act
            var response = await client.GetAsync("api/Cats");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void HttpGet_GetCats_Endpoint_Returns_Cats_When_Db_Is_Not_Empty()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.RestoreInitialState();

            // Act
            var response = await client.GetAsync("api/Cats");

            // Assert
            Cat[] result = await response.Content.ReadFromJsonAsync<Cat[]>() ?? new Cat[0];
            Assert.Equal(2, result.Length);
            Assert.Equal("Susan", result[0].Name);
            Assert.Equal("Yuki", result[1].Name);
        }

        // HttpPost SaveCat([FromBody]Cat cat) Tests:

        [Fact]
        public async void HttpPost_SaveCat_Endpoint_Saves_Cat_In_Database()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.RestoreInitialState();
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
        public async void HttpPost_SaveCat_Endpoint_Returns_Saved_Cat()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.RestoreInitialState();
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

            var resultSavedCat = context.Cats.ToList().Last();
            Cat resultResponseCat = await response.Content.ReadFromJsonAsync<Cat>() ?? new Cat();
            
            Assert.Equal(resultSavedCat.Name, resultResponseCat.Name);
            Assert.Equal(resultSavedCat.Id, resultResponseCat.Id);
            Assert.Equal(resultSavedCat.IsVaccinated, resultResponseCat.IsVaccinated);
            Assert.Equal(resultSavedCat.IsSterilized, resultResponseCat.IsSterilized);
            Assert.Equal(resultSavedCat.HasChip, resultResponseCat.HasChip);
            Assert.Equal(resultSavedCat.DateOfBirth, resultResponseCat.DateOfBirth);
        }
    }
}