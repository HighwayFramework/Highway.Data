using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Highway.Data.Contexts
{
    public interface IIdentityStrategy<T>
            where T : class
    {
        void Assign(T entity);
    }
}
