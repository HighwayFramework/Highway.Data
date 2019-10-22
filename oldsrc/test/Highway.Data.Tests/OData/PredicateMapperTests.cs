// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredicateMapperTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the PredicateMapperTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.OData;
using NUnit.Framework;

namespace Highway.Data.Tests.OData
{
    public class PredicateMapperTests
    {
        public class PredicateConverterTests
        {
            public class GivenAPredicateMapper
            {
                [Test]
                public void CanConvert()
                {
                    var converter = PredicateMapper.Map<InDto, OutDto>();
                    Expression<Func<InDto, bool>> expression = x => x.Name == "foo";

                    var converted = converter.Convert<InDto, OutDto>(expression);

                    Assert.AreEqual("x => (x.Name == \"foo\")", converted.ToString());
                }

                [Test]
                public void CanConvertOnChildType()
                {
                    var converter = PredicateMapper.Map<InDto, OutDto>()
                        .And<ChildInDto, ChildOutDto>();

                    Expression<Func<InDto, bool>> expression = x => x.Children.Any(y => y.Value == "foo");

                    var converted = converter.Convert<InDto, OutDto>(expression);

                    Assert.AreEqual("x => x.Children.Any(y => (y.Value == \"foo\"))", converted.ToString());
                }

                [Test]
                public void CanConvertWithSubstitution()
                {
                    var converter = PredicateMapper.Map<InDto, AliasDto>().MapMember<InDto, AliasDto, string>(x => x.Name, x => x.Alias);

                    Expression<Func<InDto, bool>> expression = x => x.Name == "foo";

                    Expression<Func<AliasDto, bool>> converted = converter.Convert<InDto, AliasDto>(expression);

                    Assert.AreEqual("x => (x.Alias == \"foo\")", converted.ToString());
                }

                [Test]
                public void WhenSubstitutionSourceIsNotMemberExpressionThenThrows()
                {
                    Assert.Throws<ArgumentException>(() => PredicateMapper.Map<InDto, AliasDto>().MapMember<InDto, AliasDto, string>(x => x.GetName(), x => x.Alias));
                }

                [Test]
                public void WhenSubstitutionTargetIsNotMemberExpressionThenThrows()
                {
                    Assert.Throws<ArgumentException>(() => PredicateMapper.Map<InDto, AliasDto>().MapMember<InDto, AliasDto, string>(x => x.Name, x => x.GetAlias()));
                }
            }

            public class InDto
            {
                public string Name { get; set; }

                public IEnumerable<ChildInDto> Children { get; set; }

                public string GetName()
                {
                    return Name;
                }
            }

            public class OutDto
            {
                public string Name { get; set; }

                public IEnumerable<ChildOutDto> Children { get; set; }
            }

            public class ChildInDto
            {
                public string Value { get; set; }
            }

            public class ChildOutDto
            {
                public string Value { get; set; }
            }

            public class AliasDto
            {
                public string Alias { get; set; }

                public string GetAlias()
                {
                    return Alias;
                }
            }
        }
    }
}