#region

using System.Collections.Generic;

#endregion

namespace Highway.Data.Filtering
{
    public class AndCriteria : CombinationCriteria
    {
        private readonly Criteria _leftCriteria;
        private readonly Criteria _rightCriteria;

        internal AndCriteria(Criteria leftCriteria, Criteria rightCriteria)
        {
            _leftCriteria = leftCriteria;
            _rightCriteria = rightCriteria;
            _rightCriteria.ArgumentNumber += _leftCriteria.ArgumentNumber;
        }

        public override string GetFilterString()
        {
            return string.Format("({0} && {1})", _leftCriteria.GetFilterString(), _rightCriteria.GetFilterString());
        }

        public override object[] GetFilterArguments()
        {
            var args = new List<object>();
            args.AddRange(_leftCriteria.GetFilterArguments());
            args.AddRange(_rightCriteria.GetFilterArguments());
            return args.ToArray();
        }
    }
}