namespace BuilderTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;
    using static Data.TestData;

    public class Combine
    {
        [Fact]
        public void IgnoreCase()
        {
            var result = DataSet.Build(
                    _ => _.Where(x => x.Id > 1)
                        .And.Contains(x => x.Name, "BAB"))
                .ToList();

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id > 1 && x.Name.ToLower().Contains("bab")));
        }

        [Fact]
        public void IgnoreCaseIgnoreDefault()
        {
            var result = DataSet.Build(
                    _ => _.Where(x => x.Id > 1)
                        .And.Contains(x => x.Name, null))
                .ToList();

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id > 1));
        }

        [Theory]
        [ClassData(typeof(ThreeX))]
        public void CombineAnd(bool x1, bool x2, bool x3)
        {
            bool result = x1 && x2 && x3;
            Evaluate(_ => _.Where(x => x1).And.Where(x => x2).And.Where(x => x3)).Should().Be(result);
        }

        [Theory]
        [ClassData(typeof(ThreeX))]
        public void CombineOr(bool x1, bool x2, bool x3)
        {
            bool result = x1 || x2 || x3;
            Evaluate(_ => _.Where(x => x1).Or.Where(x => x2).Or.Where(x => x3)).Should().Be(result);
        }

        [Theory]
        [ClassData(typeof(ThreeX))]
        public void CombineCondition(bool x1, bool x2, bool x3)
        {
            bool result = !(x1 && x2 && x3);
            Evaluate(_ => _
                .Conditional(x1).Conditional(x2).Conditional(x3).Where(x => false)
                .And.Where(x => true)).Should().Be(result);
        }

        [Theory]
        [ClassData(typeof(FourX))]
        public void PrecedenceAndOrAnd(bool x1, bool x2, bool x3, bool x4)
        {
            bool result = (x1 && x2) || (x3 && x4);
            Evaluate(_ => _
                .Brackets(b => b.Where(x => x1).And.Where(x => x2))
                .Or.Brackets(b => b.Where(x => x3).And.Where(x => x4))).Should().Be(result);
        }

        [Theory]
        [ClassData(typeof(FourX))]
        public void PrecedenceOrAndOr(bool x1, bool x2, bool x3, bool x4)
        {
            bool result = (x1 || x2) && (x3 || x4);
            Evaluate(_ => _
                .Brackets(b => b.Where(x => x1).Or.Where(x => x2))
                .And.Brackets(b => b.Where(x => x3).Or.Where(x => x4))).Should().Be(result);
        }

        [Theory]
        [ClassData(typeof(FourX))]
        public void PrecedenceAndAndAnd(bool x1, bool x2, bool x3, bool x4)
        {
            bool result = (x1 && x2) && (x3 && x4);
            Evaluate(_ => _
                .Brackets(b => b.Where(x => x1).And.Where(x => x2))
                .And.Brackets(b => b.Where(x => x3).And.Where(x => x4))).Should().Be(result);
        }

        [Theory]
        [ClassData(typeof(FourX))]
        public void PrecedenceOrOrOr(bool x1, bool x2, bool x3, bool x4)
        {
            bool result = (x1 || x2) || (x3 || x4);
            Evaluate(_ => _
                .Brackets(b => b.Where(x => x1).Or.Where(x => x2))
                .Or.Brackets(b => b.Where(x => x3).Or.Where(x => x4))).Should().Be(result);
        }

        private static bool X(int val, int mask)
        {
            return (val & mask) != 0;
        }

        private bool Evaluate(
            Func<IAndOrOperator<long>, IResult<long>> builder)
        {
            _ = builder ?? throw new ArgumentException("Builder cannot be null", nameof(builder));

            var expression = QueryableBuilderExtensions.CreateExpression(builder, new OperationStrategy());
            Debug.WriteLine(expression.ToString());

            return expression.Compile().Invoke(1);
        }

        private class ThreeX : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                for (int i = 0; i < 0b111; i++)
                {
                    yield return new object[] { X(i, 0b001), X(i, 0b010), X(i, 0b100) };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class FourX : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                for (int i = 0; i < 0b1111; i++)
                {
                    yield return new object[] { X(i, 0b0001), X(i, 0b0010), X(i, 0b0100), X(i, 0b1000) };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}