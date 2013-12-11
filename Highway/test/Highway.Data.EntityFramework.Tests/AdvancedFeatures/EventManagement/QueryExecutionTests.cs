using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using FluentAssertions;
using Highway.Data.EntityFramework.Tests.EventManagement;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Factories;
using Highway.Data.Interceptors;
using Highway.Data.Interceptors.Events;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement
{
    [TestClass]
    public class QueryExecutionTests
    {
        [TestMethod]
        public void ShouldAddSingleInterceptorForPreQueryExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<BeforeQuery>(1);
            domain.Events = new List<IInterceptor>
            {
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new RepositoryFactory(new []{domain}).Create<TestDomain>();
            repository.Find(new EmptyQuery());

            //assert
            interceptor.WasCalled.Should().BeTrue();

        }

        [TestMethod]
        public void ShouldAddTwoInterceptorForPreQueryExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<BeforeQuery>(1);
            var interceptorTwo = new TestEventInterceptor<BeforeQuery>(2);
            domain.Events = new List<IInterceptor>
            {
                interceptorTwo,
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new RepositoryFactory(new []{domain}).Create<TestDomain>();
            repository.Find(new EmptyQuery());

            //assert
            interceptor.WasCalled.Should().BeTrue();
            interceptorTwo.WasCalled.Should().BeTrue();
            interceptor.CallTime.Should().BeBefore(interceptorTwo.CallTime);

        }

        [TestMethod]
        public void ShouldAddSingleInterceptorForAfterQueryExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<AfterQuery>(1);
            domain.Events = new List<IInterceptor>
            {
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new RepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Find(new EmptyQuery());

            //assert
            interceptor.WasCalled.Should().BeTrue();

        }

        [TestMethod]
        public void ShouldAddTwoInterceptorForAfterQueryExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<AfterQuery>(1);
            var interceptorTwo = new TestEventInterceptor<AfterQuery>(2);
            domain.Events = new List<IInterceptor>
            {
                interceptorTwo,
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new RepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Find(new EmptyQuery());

            //assert
            interceptor.WasCalled.Should().BeTrue();
            interceptorTwo.WasCalled.Should().BeTrue();
            interceptor.CallTime.Should().BeBefore(interceptorTwo.CallTime);

        }

        [TestMethod]
        public void ShouldModifyTheQueryInExpressionToHaveAnAdditionalWhere()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new AppendWhere();
            domain.Events = new List<IInterceptor>
            {
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new RepositoryFactory(new[] { domain }).Create<TestDomain>();
            var emptyQuery = new EmptyQuery();
            repository.Find(emptyQuery);

            //assert
            PropertyInfo propertyInfo = typeof (EmptyQuery).GetProperty("ContextQuery",BindingFlags.NonPublic | BindingFlags.Instance);
            var field = propertyInfo.GetValue(emptyQuery);
        }
    }

    public class AppendWhere : IEventInterceptor<BeforeQuery>
    {
        public AppendWhere()
        {
            Priority = 1;
        }
        public InterceptorResult Apply(IDataContext context, BeforeQuery eventArgs)
        {
            var query = eventArgs.Query as EmptyQuery;
            var func = ExtractQuery<EmptyQuery, Foo>(query);
            return InterceptorResult.Succeeded();
        }

        private Func<IDataContext, IQueryable<TK>> ExtractQuery<T, TK>(T query) where T : IQueryBase
        {
            var fieldDescription = typeof (T).GetProperty("ContextQuery", BindingFlags.NonPublic | BindingFlags.Instance);
            object value = fieldDescription.GetValue(query);
            return value as Func<IDataContext,IQueryable<TK>>;
        }

        public int Priority { get; private set; }
    }

    public class EmptyQuery : Query<Foo>
    {
        public EmptyQuery()
        {
            ContextQuery = c=> new List<Foo>().AsQueryable();
        }
    }
}