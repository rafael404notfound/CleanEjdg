namespace CleanEjdg.Tests.Application.UnitTests {
    
    public class CatServiceTests {
        [Theory]
        [InlineData(0, 1, 2023, 4)]
        [InlineData(0, 11, 2022, 6)]
        [InlineData(2, 10, 2020, 7)]
        public void CatAge_Method_Returns_Correct_Value(int expectedYears, int expectedMonths, int yearOfBirth, int monthOfBirth) {
            
            //Arrange
            var dateTimeMock = new Mock<IDateTime>();
            dateTimeMock.Setup(m => m.Now).Returns(new DateTime(2023, 5, 20));

            var cat = new Cat{
                Name = "c1",
                DateOfBirth = new  DateTime(yearOfBirth, monthOfBirth, 1)
            };

            var sut = new CatService(dateTimeMock.Object);
            
            //Act
            IDictionary<string, int> result = sut.CatAge(cat);
            
            //Assert            
            Assert.Equal(expectedYears, result["Years"]);
            Assert.Equal(expectedMonths, result["Months"]);
        }
    }
}