using System;
using System.Collections.Generic;
using System.Reflection;

namespace Highway.Data.OData
{
    internal class PredicateConverter<TSource, TResult> : IPredicateConverter
    {
        private readonly IDictionary<MemberInfo, MemberInfo> _substitutions = new Dictionary<MemberInfo, MemberInfo>();

        public Type SourceType
        {
            get { return typeof (TSource); }
        }

        public Type TargetType
        {
            get { return typeof (TResult); }
        }

        public IDictionary<MemberInfo, MemberInfo> Substitutions
        {
            get { return _substitutions; }
        }
    }
}