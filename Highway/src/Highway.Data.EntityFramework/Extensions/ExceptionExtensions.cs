using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Highway.Data.EntityFramework
{
    public static class ExceptionExtensions
    {
        public static string FullMessage(this Exception exception)
        {
            return String.Join(" ", exception
                .NestedExceptions()
                .Select(x => x.MeaningfulMessage()));
        }

        public static string MeaningfulMessage(this Exception exception)
        {
            var result = exception.Message;

            var validationException = exception as DbEntityValidationException;

            if (validationException != null)
            {
                var errors =
                    from entity in validationException.EntityValidationErrors
                    from error in entity.ValidationErrors
                    select $"{error.ErrorMessage} Property name: {entity.Entry.Entity.GetType().Name}.{error.PropertyName}.";

                result = String.Join(", ", errors);
            }

            return result;
        }

        public static IEnumerable<Exception> NestedExceptions(this Exception exception)
        {
            while (exception != null)
            {
                yield return exception;
                exception = exception.InnerException;
            }
        }
    }
}
