using Moq;
using System;
using TestProject.Models.DataModels;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using MoqEFCoreExtension;
using Microsoft.EntityFrameworkCore;

namespace TestProject.XUnitTest
{
    [Trait("TestProject", "ClassRepository≤‚ ‘")]
    public class ClassRepositoryTest
    {
        Mock<TestManageDBContext> _dbMock;
        IClassRepository _clsRep;
        public ClassRepositoryTest()
        {
            _dbMock = new Mock<TestManageDBContext>();
            _clsRep = new ClassRepository(_dbMock.Object);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void AddClass_Default_ReturnTrue(int result)
        {

            _dbMock.Setup(db => db.Classes.Add(new Classes()));
            _dbMock.Setup(db => db.SaveChanges()).Returns(value: result);
            var backResult = _clsRep.AddClass(new Classes { Id = 1 });
            Assert.Equal(result == 1, backResult);
        }

        [Fact]
        public void AddClass_AddNull_ThrowException()
        {
            var ext = Assert.Throws<Exception>(() => _clsRep.AddClass(null));
            Assert.Contains("∞‡º∂≤ªƒ‹Œ™Null", ext.Message);

        }

        [Fact]
        public void AddClass_Default_SavaThrowException()
        {
            _dbMock.Setup(db => db.Classes.Add(new Classes()));
            _dbMock.Setup(db => db.SaveChanges()).Throws(new Exception("Õ¯¬Áπ ’œ"));
            var ext = Assert.Throws<Exception>(() => _clsRep.AddClass(new Classes()));
            Assert.Contains("Õ¯¬Áπ ’œ", ext.Message);
        }

        [Fact]
        public void GetClass_Default_Return()
        {
            var list = new List<Classes>()
            {
                new Classes {  Id=1, ClassName="“ª∞‡",Memo="" },
                new Classes {  Id=2, ClassName="∂˛∞‡", Memo=""}
            };

            var clsSet = new Mock<DbSet<Classes>>().SetupList(list);
            _dbMock.Setup(db => db.Classes).Returns(clsSet.Object);
            var clses = _clsRep.GetClasses();
            Assert.Equal(2, clses.Count);
        }



    }
}
