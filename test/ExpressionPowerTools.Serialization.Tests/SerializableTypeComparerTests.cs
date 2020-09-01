﻿using System;
using System.Collections.Generic;
using ExpressionPowerTools.Serialization.Serializers;
using Xunit;

namespace ExpressionPowerTools.Serialization.Tests
{
    public class SerializableTypeComparerTests
    {
        public readonly IEqualityComparer<SerializableType> comparer =
            new SerializableTypeComparer();

        public static IEnumerable<object[]> GetComparisonMatrix()
        {
            yield return new object[]
            {
                ReflectionHelper.Instance.SerializeType(typeof(int)),
                ReflectionHelper.Instance.SerializeType(typeof(int)),
                true
            };

            yield return new object[]
            {
                ReflectionHelper.Instance.SerializeType(typeof(int)),
                ReflectionHelper.Instance.SerializeType(typeof(long)),
                false
            };

            yield return new object[]
            {
                default(SerializableType),
                ReflectionHelper.Instance.SerializeType(typeof(int)),
                false
            };

            yield return new object[]
            {
                ReflectionHelper.Instance.SerializeType(typeof(int)),
                default(SerializableType),
                false
            };

            yield return new object[]
            {
                default(SerializableType),
                default(SerializableType),
                true
            };

            yield return new object[]
            {
                ReflectionHelper.Instance.SerializeType(typeof(IComparable<int>)),
                ReflectionHelper.Instance.SerializeType(typeof(IComparable<int>)),
                true
            };

            yield return new object[]
            {
                ReflectionHelper.Instance.SerializeType(typeof(IComparable<int>)),
                ReflectionHelper.Instance.SerializeType(typeof(IComparable<long>)),
                false
            };

            yield return new object[]
            {
                ReflectionHelper.Instance.SerializeType(typeof(IComparable<int>)),
                ReflectionHelper.Instance.SerializeType(typeof(IComparable<>)),
                false
            };

            yield return new object[]
            {
                ReflectionHelper.Instance.SerializeType(typeof(IComparable<>)),
                ReflectionHelper.Instance.SerializeType(typeof(IComparable<>)),
                true
            };

            yield return new object[]
            {
                new SerializableType { TypeParamName = nameof(SerializableTypeComparerTests) },
                new SerializableType { TypeParamName = nameof(SerializableTypeComparerTests) },
                true
            };

            yield return new object[]
            {
                new SerializableType { TypeParamName = nameof(SerializableTypeComparerTests) },
                new SerializableType { TypeParamName = nameof(GetComparisonMatrix) },
                false
            };
        }

        [Theory]
        [MemberData(nameof(GetComparisonMatrix))]
        public void GivenTwoSerializableTypesWhenComparableEqualsCalledThenShouldResolveEquality(
            SerializableType source,
            SerializableType target,
            bool areEqual)
        {
            Assert.Equal(areEqual, comparer.Equals(source, target));
        }

        [Fact]
        public void GivenComparerWhenGetHashCodeCalledThenCallsGetHashCodeOnSerializableType()
        {
            var type = ReflectionHelper.Instance.SerializeType(typeof(IComparable<IEqualityComparer<int>>));
            Assert.Equal(type.GetHashCode(), comparer.GetHashCode(type));
        }
    }
}
