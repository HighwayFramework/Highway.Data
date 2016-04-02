using System.Collections.Concurrent;

namespace Highway.Data.EntityFramework.Interceptors.DynamicFilters
{
    public class DynamicFilterParameters
    {
        //  Null will default to true but allows us to specifically enable/disable on the local scope level to
        //  override a filter that may be globally disabled.
        public bool? Enabled { get; set; }

        public ConcurrentDictionary<string, object> ParameterValues { get; private set; }

        public DynamicFilterParameters()
        {
            ParameterValues = new ConcurrentDictionary<string, object>();
        }

        public void SetParameter(string parameterName, object value)
        {
            ParameterValues.AddOrUpdate(parameterName, value, (k, v) => value);
        }
    }
}
