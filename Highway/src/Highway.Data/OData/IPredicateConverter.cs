using System;
using System.Collections.Generic;
using System.Reflection;

namespace Highway.Data.OData
{
    internal interface IPredicateConverter
    {
        Type SourceType { get; }
        Type TargetType { get; }
        IDictionary<MemberInfo, MemberInfo> Substitutions { get; }
    }
}