using FrameworkExtension.Core.Contexts;
using FrameworkExtension.Core.Test.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FrameworkExtension.Core.Test.EntityFramework.UnitTests
{
    [TestClass]
    public class Given_An_Event_Extendable_Context
    {
        [TestMethod]
        public void When_Commit_Is_Called_PreSave_and_post_save_interceptors_are_Called()
        {
            //arrange
            var context = new EntityFrameworkContext("Test", mockEventManager);

            //Act
            IPreSaveInterceptor mockPreSave = MockRepository.GenerateStrictMock<IPreSaveInterceptor>();
            mockPreSave.Expect(x => x.Execute(context)).Returns(true);
            context.EventManager.Register(mockPreSave);
            
            IPostSaveInterceptor mockPostSave = MockRepository.GenerateStrictMock<IPostSaveInterceptor>();
            mockPostSave.Expect(x => x.Execute(context)).Returns(true);
            context.EventManager.Register(mockPostSave);
            context.Add(new Foo());
            context.Commit();
            
            //Assert
            mockPostSave.VerifyAllExpectations();
            mockPreSave.VerifyAllExpectations();
        }

    }
}