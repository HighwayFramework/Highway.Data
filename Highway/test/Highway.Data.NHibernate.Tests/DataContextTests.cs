using System;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using Highway.Data.NHibernate.Tests.Properties;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Impl;

namespace Highway.Data.NHibernate.Tests
{
    [TestClass]
    public class DataContextTests
    {
        [TestMethod]
        public void ShouldReturnAnUpdatedObject()
        {
            //Arrange
            var sessionFactory = TastFactory.CreateSessionFactory();
            var target = new DataContext(sessionFactory.OpenSession());

            //Act 
            var item = new Foo();
            target.Add(item);
            target.Commit();

            //Assert
            Assert.IsNotNull(item.Id);
            Assert.AreNotEqual(default(int), item.Id);
            Console.WriteLine(item.Id);
        }

        [TestMethod]
        public void Should_Be_Able_To_Run_Query()
        {
            //Arrange
            var sessionFactory = TastFactory.CreateSessionFactory();
            var target = new DataContext(sessionFactory.OpenSession());
            
            //Act
            var result = new FindFoo().Execute(target).ToList();

            //Assert
            Assert.IsTrue(result.Any());
        }
    }

    public class TastFactory
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                              .ConnectionString(
                                  c => c.Is(Settings.Default.Connection)))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<FooMap>())
                .BuildSessionFactory();
        }
    }

    public class FooMap : ClassMap<Foo>
    {
        public FooMap()
        {
            Table("Foos");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Address);
        }
    }
}
