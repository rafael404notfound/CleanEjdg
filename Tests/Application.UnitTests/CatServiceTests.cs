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
            var catRepoMock = new Mock<ICatRepository>();

            var cat = new Cat{
                Name = "c1",
                DateOfBirth = new  DateTime(yearOfBirth, monthOfBirth, 1)
            };

            var sut = new CatService(dateTimeMock.Object, catRepoMock.Object);
            
            //Act
            IDictionary<string, int> result = sut.CatAge(cat);
            
            //Assert            
            Assert.Equal(expectedYears, result["Years"]);
            Assert.Equal(expectedMonths, result["Months"]);
        }

        /*
        [Fact]
        public void GetCats_Return_All_Cats_In_Database()
        {
            //Arrange       
            var catRepoMock = new Mock<ICatRepository>();
            catRepoMock.Setup(m => m.GetAll()).Returns((new Cat[] {
                new Cat { Id = 1, DateOfBirth = new DateTime(2021, 8, 12), Name = "c1" },
                new Cat { Id = 2, DateOfBirth = new DateTime(2018, 2, 24), Name = "c2" },
                new Cat { Id = 3, DateOfBirth = new DateTime(2023, 4, 6), Name = "c3" }
                }).AsQueryable<Cat>);
            DateTimeServer dateTimeServer = new DateTimeServer();
            CatService sut = new CatService(dateTimeServer, catRepoMock.Object);
            //Act
            IEnumerable<Cat>? result = sut.GetAllCats();

            //Assert
            Cat[] catArray = result.ToArray() ?? Array.Empty<Cat>();
            Assert.True(catArray.Length == 3);
            Assert.Equal("c1", catArray[0].Name);
            Assert.Equal("c2", catArray[1].Name);
            Assert.Equal("c3", catArray[2].Name);
        }
        */
    }
}