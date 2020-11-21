using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProductCatalog.Domain;
using ProductCatalog.EFRepositories;
using ProductCatalog.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductCatalog.BusinessObjectsTests
{
    [TestClass]
    public class BookBOTest
    {
        [TestMethod()]
        public void Get_BookObjectPassed_ProperMethodCalled()
        {
            var testObject = new Book();

            var context = new Mock<ProductCatalogContext>();
            var dbSetMock = new Mock<DbSet<Book>>();

            context.Setup(x => x.Set<Book>()).Returns(dbSetMock.Object);
            dbSetMock.Setup(x => x.Find(It.IsAny<int>())).Returns(testObject);

            // Act
            var repository = new GenericRepository<Book>(context.Object);
            repository.GetDetails(1);

            // Assert
            context.Verify(x => x.Set<Book>());
            dbSetMock.Verify(x => x.FindAsync(It.IsAny<int>()), Times.Once);

        }

        [TestMethod]
        public void GetAll_BookObjectPassed_ProperMethodCalled()
        {
            // Arrange
            var testBookObject = new Book() { Id = 1 };
            var testList = new List<Book>() { testBookObject };

            var dbSetMock = new Mock<DbSet<Book>>();
            dbSetMock.As<IQueryable<Book>>().Setup(x => x.Provider).Returns(testList.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Book>>().Setup(x => x.Expression).Returns(testList.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Book>>().Setup(x => x.ElementType).Returns(testList.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Book>>().Setup(x => x.GetEnumerator()).Returns(testList.AsQueryable().GetEnumerator());

            var context = new Mock<DbContext>();
            context.Setup(x => x.Set<Book>()).Returns(dbSetMock.Object);

            // Act
            var repository = new GenericRepository<Book>(context.Object);
            var result = repository.GetAll().Result.ToList();

            // Assert
            Assert.Equals(testList, result.ToList());
        }
    }
}

