using FrameworkExtension.Core.Contexts;
using FrameworkExtension.Core.Test.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Microsoft.Practices.ServiceLocation;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Castle.MicroKernel.Registration;
using System.Net;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.EventArgs;

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
            var mockEventManager = MockRepository.GenerateMock<IEventManager>();
            container.Register(Component.For<IEventManager>().Instance(mockEventManager),
                               Component.For<EntityFrameworkContext>());
            var context = container.Resolve<EntityFrameworkContext>(new { connectionString = "Test" });

            //Act
            IInterceptor<PreSaveEventArgs> mockPreSave = MockRepository.GenerateStrictMock<IInterceptor<PreSaveEventArgs>>();
            mockPreSave.Expect(x => x.Execute(context, new PreSaveEventArgs())).Return(true);
            context.EventManager.Register(mockPreSave);

            IInterceptor<PostSaveEventArgs> mockPostSave = MockRepository.GenerateStrictMock<IInterceptor<PostSaveEventArgs>>();
            mockPostSave.Expect(x => x.Execute(context, new PostSaveEventArgs())).Return(true);
            context.EventManager.Register(mockPostSave);
            context.Add(new Foo());
            context.Commit();

            //Assert
            mockPostSave.VerifyAllExpectations();
            mockPreSave.VerifyAllExpectations();
        }

    }
}