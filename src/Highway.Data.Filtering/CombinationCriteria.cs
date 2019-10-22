using System;

namespace Highway.Data
{
    /// <summary>
    ///     Base abstract class for combining criteria together
    /// </summary>
    public abstract class CombinationCriteria : Criteria
    {
        private readonly Criteria _leftCriteria;
        private readonly Criteria _rightCriteria;

        public override int ArgumentNumber
        {
            get { return _leftCriteria.ArgumentNumber + _rightCriteria.ArgumentNumber; }
            set
            {
                _leftCriteria.ArgumentNumber += value;
                _rightCriteria.ArgumentNumber += _leftCriteria.ArgumentNumber;
            }
        }
    }
}