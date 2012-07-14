using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Rhino.Mocks;
using Castle.MicroKernel.Resolvers;
using System.Collections;

namespace Highway.Data.Tests
{
    public class AutoMockingLazyComponentLoader : ILazyComponentLoader
    {
        public IRegistration Load(string key, Type service, IDictionary arguments)
        {
            //if (service.IsArray)
            //{
            //    var elementType = service.GetElementType();
            //    object mock = MockRepository.GenerateMock(elementType, new Type[0]);
            //    Array array = Array.CreateInstance(elementType, 1);
            //    array.SetValue(mock, 0);
            //    return Component.For(service).Instance(array);
            //}
            return Component.For(service).Instance(MockRepository.GenerateMock(service, new Type[0]));
        }
    }
}
