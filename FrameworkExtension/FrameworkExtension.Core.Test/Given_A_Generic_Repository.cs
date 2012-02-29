using System;
using System.Data.Entity;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrameworkExtension.Core.Test
{
    [TestClass]
    public class Given_A_Generic_Repository
    {
        [TestMethod]
        public void When_Created__It_Creates_The_Proper_Context()
        {
            //Arrange 

            //act
            var repository = new EntityFrameworkRepository<EFTestContext>();

            //Assert
            Assert.IsInstanceOfType(repository.Context, typeof(EFTestContext));

        }
    }

    public class EFTestContext : DbContext, IDbContext
    {
        public EFTestContext(string connectionString) : base(connectionString)
        {
            
        }
    }
}
