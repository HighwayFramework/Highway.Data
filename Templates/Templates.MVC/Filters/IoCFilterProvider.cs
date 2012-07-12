using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Templates.Filters
{
    public class IoCFilterProvider : IFilterProvider
    {
        private readonly IEnumerable<Func<ControllerContext, ActionDescriptor, Filter>> registeredFilters;

        public IoCFilterProvider(Func<ControllerContext, ActionDescriptor, Filter>[] registeredFilters)
        {
            this.registeredFilters = registeredFilters;
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return registeredFilters.Select(m => m.Invoke(controllerContext, actionDescriptor)).Where(m => m != null);
        }
    }
}
