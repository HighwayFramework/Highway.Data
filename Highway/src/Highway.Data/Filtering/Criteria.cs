namespace Highway.Data.Filtering
{
    /// <summary>
    /// Base abstract Criteria
    /// </summary>
    public abstract class Criteria
    {
        public abstract string GetFilterString();
        public abstract object[] GetFilterArguments();
        public virtual int ArgumentNumber { get; set; }

        public static FieldIdentifier<T> Field<T>(string fieldName)
        {
            return new FieldIdentifier<T>(fieldName);
        }
    }
}