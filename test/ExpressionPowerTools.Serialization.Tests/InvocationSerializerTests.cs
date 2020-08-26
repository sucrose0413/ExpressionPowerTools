﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ExpressionPowerTools.Serialization.Serializers;
using ExpressionPowerTools.Serialization.Tests.TestHelpers;
using Xunit;

namespace ExpressionPowerTools.Serialization.Tests
{
    public class InvocationSerializerTests
    {
        private readonly InvocationSerializer invocationSerializer =
            new InvocationSerializer(TestSerializer.ExpressionSerializer);

        private static readonly Expression<Func<int, bool>> FuncIntBool = i => true;

        private static readonly Expression<Func<bool>> FuncBool = () => true;

        private static readonly Expression<Func<string>> FuncString = () => nameof(FuncString);

        // TODO
        // private static readonly Expression<Func<long,bool>> FuncLongBool = i => i > int.MaxValue;

        public static IEnumerable<object[]> GetInvocationExpressionMatrix()
        {

            // left equivalent, right not equivalent (false)
            yield return new object[]
            {
                Expression.Invoke(FuncBool, FuncBool.Parameters)
            };

            yield return new object[]
            {
                Expression.Invoke(FuncString, FuncString.Parameters)
            };

            yield return new object[]
            {
                Expression.Invoke(FuncIntBool, FuncIntBool.Parameters)
            };
        }

        [Theory]
        [MemberData(nameof(GetInvocationExpressionMatrix))]
        public void InvocationExpressionShouldSerialize(InvocationExpression invocation)
        {
            var target = invocationSerializer.Serialize(invocation);
            Assert.Equal(target.Type, invocation.NodeType.ToString());
        }

        [Theory]
        [MemberData(nameof(GetInvocationExpressionMatrix))]
        public void InvocationExpressionShouldDeserialize(InvocationExpression invocation)
        {
            var serialized = TestSerializer.GetSerializedFragment<Invocation,
                InvocationExpression>(invocation);
            var deserialized = invocationSerializer.Deserialize(serialized);
            Assert.Equal(invocation.Type.FullName, deserialized.Type.FullName);
        }
    }
}
