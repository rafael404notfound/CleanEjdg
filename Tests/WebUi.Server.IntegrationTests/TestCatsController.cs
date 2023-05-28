
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests {
    
    public class TestCatsController : IClassFixture<IntegrationTestFactory> 
    {
        private readonly IntegrationTestFactory _factory;

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

        private readonly IEnumerable<Cat> TestCatsWithId = new List<Cat>()
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

        public TestCatsController(IntegrationTestFactory factory)
        {
            _factory = factory;
        }

        // HttpGet GetCats() Tests:

        [Fact]
        public async void HttpGet_GetCats_Endpoint_Returns_Success_When_Cats_Are_Returned()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.GetAsync("api/Cats");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void HttpGet_GetCats_Endpoint_Returns_Cats()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.GetAsync("api/Cats");

            // Assert
            Cat[] result = await response.Content.ReadFromJsonAsync<Cat[]>() ?? new Cat[0];
            Assert.Equal(2, result.Length);
            Assert.Equal("Susan", result[0].Name);
            Assert.Equal("Yuki", result[1].Name);
        }

        [Fact]
        public async void HttpGet_GetCats_Endpoint_Returns_404NotFound_When_Cats_Are_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(Enumerable.Empty<Cat>());

            // Act
            var response = await client.GetAsync("api/Cats");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        // HttpGet GetCat(int id) Tests:

        [Fact]
        public async void HttpGet_GetCat_Endpoint_Returns_Succes_When_Cat_Is_Returned()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCatsWithId);

            // Act
            var response = await client.GetAsync("api/Cats/1");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void HttpGet_GetCat_Endpoint_Returns_Cat()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCatsWithId);

            // Act
            var response = await client.GetAsync("api/Cats/1");

            // Assert
            Cat result = await response.Content.ReadFromJsonAsync<Cat>() ?? new Cat();
            Assert.Equal("Susan", result.Name);
        }

        [Fact]
        public async void HttpGet_GetCat_Endpoint_Returns_404NotFound_When_Cat_Is_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);

            // Act
            var response = await client.GetAsync("api/Cats/3");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        // HttpPost SaveCat([FromBody]Cat cat) Tests:

        [Fact]
        public async void HttpPost_SaveCat_Endpoint_Saves_Cat()
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
        public async void HttpPost_SaveCat_Endpoint_Returns_Saved_Cat()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);
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

        [Fact]
        public async void HttpPost_SaveCat_Endpoint_Returns_Succes_When_Cat_Is_Saved()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);
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
        public async void HttpPost_SaveCat_Endpoint_Returns_BadRequest_When_Cat_Is_Not_Saved()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);
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
        public async void HttpPost_SaveCat_Endpoint_Does_Not_Save_Cat_When_Model_Is_Not_Valid()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCats);
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

        // HttpDelete DeleteCat(int id) Tests:

        [Fact]
        public async void HttpDelete_DeleteCat_Endpoint_Returns_Succes_When_Cat_Is_Deleted()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCatsWithId);

            // Act
            var response = await client.DeleteAsync("api/Cats/1");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void HttpDelete_DeleteCat_Endpoint_Deletes_Cat()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCatsWithId);

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
        public async void HttpDelete_DeleteCat_Endpoint_Returns_404_When_Cat_Is_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCatsWithId);

            // Act
            var response = await client.DeleteAsync("api/Cats/3");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // HttpPut UpdateCat([FromBody] Cat cat) Tests:

        [Fact]
        public async void HttpPut_UpdateCat_Endpoint_Returns_Succes_When_Cat_Is_Updated()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCatsWithId);
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
        public async void HttpPut_UpdateCat_Endpoint_Returns_404_When_Cat_Is_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCatsWithId);
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
        public async void HttpPut_UpdateCat_Endpoint_Returns_Updated_Cat_When_Cat_Is_Updated()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCatsWithId);
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
        public async void HttpPut_UpdateCat_Endpoint_Updates_Cat()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SetDbInitialState(TestCatsWithId);
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