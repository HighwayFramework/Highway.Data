using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Objects;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Highway.Data.EventManagement;
using Highway.Data.Interceptors;
using Highway.Data.Interceptors.Events;
using Highway.Data;
using Highway.Data.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class GivenAnEventExtendableContext : ContainerTest<CommitEventsMockContext>
    {
        public override void RegisterComponents(IWindsorContainer container)
        {
            container.Register(
                Component.For<IEventManager>()
                    .ImplementedBy<EventManager>());
            base.RegisterComponents(container);
        }

        [TestMethod]
        public void WhenCommitIsCalledPreSaveAndPostSaveInterceptorsAreCalled()
        {
            //arrange
            var mockPreSave = MockRepository.GenerateMock<IInterceptor<PreSaveEventArgs>>();
            mockPreSave.Expect(x => x.Execute(Arg<IDataContext>.Is.Same(target), Arg<PreSaveEventArgs>.Is.Anything)).
                Return(InterceptorResult.Succeeded());
            mockPreSave.Expect(x => x.Priority).Return(1);
            target.EventManager.Register(mockPreSave);

            var mockPostSave = MockRepository.GenerateMock<IInterceptor<PostSaveEventArgs>>();
            mockPostSave.Expect(x => x.Execute(Arg<IDataContext>.Is.Same(target), Arg<PostSaveEventArgs>.Is.Anything)).
                Return(InterceptorResult.Succeeded());
            mockPostSave.Expect(x => x.Priority).Return(1);
            target.EventManager.Register(mockPostSave);

            //Act
            target.Commit();

            //Assert
            mockPreSave.VerifyAllExpectations();
            mockPostSave.VerifyAllExpectations();
        }
    }

    public class CommitEventsMockContext : IObservableDataContext
    {
        private IEventManager _eventManager;

        #region IObservableDataContext Members

        public void Dispose()
        {
        }

        public IQueryable<T> AsQueryable<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public T Add<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public T Remove<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public T Update<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public T Attach<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public T Detach<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public T Reload<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public int Commit()
        {
            PreSave(this, new PreSaveEventArgs());
            //stuff
            PostSave(this, new PostSaveEventArgs());
            return 0;
        }

        public IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
        {
            throw new NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, params DbParameter[] dbParams)
        {
            throw new NotImplementedException();
        }

        public IEventManager EventManager
        {
            get { return _eventManager; }
            set
            {
                _eventManager = value;
                _eventManager.Context = this;
            }
        }

        public event EventHandler<PreSaveEventArgs> PreSave;
        public event EventHandler<PostSaveEventArgs> PostSave;

        #endregion

        public void Reload<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public int ExecuteFunction(string procedureName, params ObjectParameter[] dbParams)
        {
            throw new NotImplementedException();
        }
    }
}