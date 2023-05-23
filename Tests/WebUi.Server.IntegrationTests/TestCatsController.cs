
namespace CleanEjdg.Tests.WebUI.Server.IntegrationTests {
    
    public class TestCatsController {
        [Fact]
        public void GetCats_OnSucces_ReturnsStatusCode200(){
            //Arrange
            var catRepoMock = new Mock<IRepositoryBase<Cat>>();
            catRepoMock.Setup(m => m.GetAll()).Returns((new Cat[] {
                new Cat { Id = 1, DateOfBirth = new DateTime(2021, 8, 12), Name = "c1" },
                new Cat { Id = 2, DateOfBirth = new DateTime(2018, 2, 24), Name = "c2" },
                new Cat { Id = 3, DateOfBirth = new DateTime(2023, 4, 6), Name = "c3" }
                }).AsQueryable());
            CatsController sut = new CatsController(catRepoMock.Object);
            //Act
            var result = (OkObjectResult)sut.GetCats();
            //Assert
            Assert.Equal(200, result.StatusCode);
        }

/*        [Fact]
        public void GetCats_Returns_Cats() {
            //Arrange

            //Act
            var result = sut.GetCats()
            //Assert
            result.A
        }
*/
    }
}