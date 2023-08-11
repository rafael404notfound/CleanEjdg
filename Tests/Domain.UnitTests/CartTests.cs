using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanEjdg.Core.Domain.ValueTypes;

namespace Domain.UnitTests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            // arrange
            Product product1 = new Product { Id = 1, Name = "P1" };
            Product product2 = new Product { Id = 2, Name = "P2" };
            Cart sut = new Cart();

            // act
            sut.AddItem(product1, 1);
            sut.AddItem(product2, 2);

            // assert
            CartLine[] results = sut.Lines.ToArray();
            Assert.Equal(2, results.Length);
            Assert.Equal(product1, results[0].Product);
            Assert.Equal(product2, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_To_Existing_Lines()
        {
            // arrange
            Product product1 = new Product { Id = 1, Name = "P1" };
            Product product2 = new Product { Id = 2, Name = "P2" };
            Cart sut = new Cart();
            sut.AddItem(product1, 1);
            sut.AddItem(product2, 5);

            // act
            sut.AddItem(product1, 10);

            // assert
            CartLine[] results = sut.Lines.ToArray();
            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(product1, results[0].Product);
            Assert.Equal(5, results[1].Quantity);
            Assert.Equal(product2, results[1].Product);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            // arrange
            Product product1 = new Product { Id = 1, Name = "P1" };
            Product product2 = new Product { Id = 2, Name = "P2" };
            Cart sut = new Cart();
            sut.AddItem(product1, 1);
            sut.AddItem(product2, 5);

            // act
            sut.RemoveLine(product1);

            // assert
            CartLine[] results = sut.Lines.ToArray();
            Assert.Single(results);
            Assert.Equal(product2, results[0].Product);
            Assert.Equal(5, results[0].Quantity);
        }


        [Fact]
        public void Can_Compute_Total_Price()
        {
            // arrange
            Product product1 = new Product { Id = 1, Name = "P1", Price = 100 };
            Product product2 = new Product { Id = 2, Name = "P2", Price = 50 };
            Cart sut = new Cart();
            sut.AddItem(product1, 1);
            sut.AddItem(product2, 5);

            // act
            var result = sut.ComputeTotalValue();

            // assert
            Assert.Equal(350, result);
        }

        [Fact]
        public void Can_Clear_Line()
        {
            // arrange
            Product product1 = new Product { Id = 1, Name = "P1"};
            Product product2 = new Product { Id = 2, Name = "P2"};
            Cart sut = new Cart();
            sut.AddItem(product1, 1);
            sut.AddItem(product2, 5);

            // act
            sut.Clear();

            // assert
            Assert.Empty(sut.Lines);
        }
    }
}
