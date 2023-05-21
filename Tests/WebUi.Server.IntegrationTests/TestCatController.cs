
namespace CleanEjdg.Tests.WebUI.Server.IntegrationTests {
    
    public class TestCatController {
        [Fact]
        public void GetCats_OnSucces_ReturnsStatusCode200(){
            //Arrange
            CatController sut = new CatController();
            //Act
            var result = (OkResult)sut.GetCats();
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