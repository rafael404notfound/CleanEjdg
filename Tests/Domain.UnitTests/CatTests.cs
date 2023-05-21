namespace CleanEjdg.Tests.Domain.UnitTests {
    
    public class CatTests {
        /* *****NO LONGER NEEDED BEACAUSE AGE IS NOW CALCULATED THROUGH CATSERVICE*****
        
        [Theory]
        [InlineData(0, 1, 2023, 4)]
        [InlineData(0, 11, 2022, 6)]
        [InlineData(2, 10, 2020, 7)]
        public void Age_Property_Returns_Correct_Value(int expectedYears, int expectedMonths, int yearOfBirth, int monthOfBirth) {
            
            //Arrange
            var sut = new Cat{
                Name = "c1",
                DateOfBirth = new  DateTime(yearOfBirth, monthOfBirth, 1)
            };
            
            //Act
            IDictionary<string, int> result = sut.Age;
            
            //Assert            
            Assert.Equal(expectedYears, result["Years"]);
            Assert.Equal(expectedMonths, result["Months"]);
        }*/
    }
}