using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using Castle.Windsor;

namespace Templates.Filters
{
    public class ExceptionLoggingFilter : IExceptionFilter
    {
        public ILogger Logger { get; set; }

        public ExceptionLoggingFilter()
        {
            Logger = NullLogger.Instance;
        }
        
        public void OnException(ExceptionContext filterContext)
        {
            Logger.Error(filterContext.HttpContext.Request.Url.ToString(), filterContext.Exception);
        }
    }
}