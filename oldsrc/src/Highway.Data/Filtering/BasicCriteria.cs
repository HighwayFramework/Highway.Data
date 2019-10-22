namespace Highway.Data.Filtering
{
    internal class BasicCriteria<T> : Criteria
    {
        internal BasicCriteria(string fieldName, string operation, T value)
        {
            Field = fieldName;
            Operator = operation;
            Value = value;
            ArgumentNumber = 1;
        }

        public string Field { get; set; }
        public string Operator { get; set; }
        public T Value { get; set; }

        public override string GetFilterString()
        {
            var filterString = string.Format("{0} {1} @{2}", Field, Operator, ArgumentNumber - 1);
            return filterString;
        }

        public override object[] GetFilterArguments()
        {
            return new object[] {Value};
        }
    }
}