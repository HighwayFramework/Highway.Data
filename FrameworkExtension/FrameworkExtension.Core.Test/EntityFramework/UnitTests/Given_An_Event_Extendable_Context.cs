﻿using FrameworkExtension.Core.Contexts;
using FrameworkExtension.Core.EventManagement;
using FrameworkExtension.Core.Interceptors;
using FrameworkExtension.Core.Interceptors.Events;
using FrameworkExtension.Core.Repositories;
using FrameworkExtension.Core.Test.Properties;
using FrameworkExtension.Core.Test.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Microsoft.Practices.ServiceLocation;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Castle.MicroKernel.Registration;
using System.Net;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.Test.EntityFramework.UnitTests
{
    [TestClass]
    public class Given_An_Event_Extendable_Context
    {
        [TestMethod]
        public void When_Commit_Is_Called_PreSave_and_post_save_interceptors_are_Called()
        {
            //arrange
            var container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            var manager = new EventManager();
            container.Register(Component.For<IEventManager>().Instance(manager),
                               Component.For<IRepository>().ImplementedBy<EntityFrameworkRepository>());
            IDataContext mockContext = MockRepository.GenerateMock<IDataContext>();
            var repository = container.Resolve<IRepository>(new { context =  mockContext });

            //Act
            IInterceptor<PreSaveEventArgs> mockPreSave = MockRepository.GenerateMock<IInterceptor<PreSaveEventArgs>>();
            mockPreSave.Expect(x => x.Execute(repository, new PreSaveEventArgs())).Return(InterceptorResult.Succeeded());
            repository.EventManager.Register(mockPreSave);

            IInterceptor<PostSaveEventArgs> mockPostSave = MockRepository.GenerateMock<IInterceptor<PostSaveEventArgs>>();
            mockPostSave.Expect(x => x.Execute(repository, new PostSaveEventArgs())).Return(InterceptorResult.Succeeded());
            repository.EventManager.Register(mockPostSave);
            repository.Context.Add(new Foo());
            repository.Context.Commit();

            //Assert
            mockPostSave.VerifyAllExpectations();
            mockPreSave.VerifyAllExpectations();
        }

    }
}