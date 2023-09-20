using Highway.Data.Interceptors.Events;

namespace Highway.Data.ReadonlyTests
{
    internal abstract class QueryWithEvents<T> : Query<T>
    {
        protected QueryWithEvents()
        {
            AfterQuery += OnAfterQuery;
            BeforeQuery += OnBeforeQuery;
        }

        private void OnBeforeQuery(object sender, BeforeQuery e)
        {
            BeforeQueryFired = true;
        }

        public bool BeforeQueryFired { get; set; }

        private void OnAfterQuery(object sender, AfterQuery e)
        {
            AfterQueryFired = true;
        }

        public bool AfterQueryFired { get; set; }
    }
}