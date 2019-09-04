using System;

namespace Highway.Data
{
    /// <summary>
    ///     Base abstract Criteria
    /// </summary>
    public abstract class Criteria
    {
        public virtual int ArgumentNumber { get; set; }
        public abstract string GetFilterString();
        public abstract object[] GetFilterArguments();

        public static FieldIdentifier<T> Field<T>(string fieldName)
        {
            return new FieldIdentifier<T>(fieldName);
        }
    }
}