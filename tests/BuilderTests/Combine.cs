namespace BuilderTests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
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
        [InlineData(true, true, true, true)]
        [InlineData(false, true, true, false)]
        [InlineData(true, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(false, false, false, false)]
        public void CombineAnd(bool x1, bool x2, bool x3, bool result)
        {
            Build(_ => _.Where(x => x1).And.Where(x => x2).And.Where(x => x3)).Should().Be(result);
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, false, false, true)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, true, true)]
        [InlineData(false, false, false, false)]
        public void CombineOr(bool x1, bool x2, bool x3, bool result)
        {
            Build(_ => _.Where(x => x1).Or.Where(x => x2).Or.Where(x => x3)).Should().Be(result);
        }

        [Theory]
        [InlineData(true, true, true, false)]
        [InlineData(false, true, true, true)]
        [InlineData(true, false, true, true)]
        [InlineData(true, true, false, true)]
        [InlineData(false, false, false, true)]
        public void CombineCondition(bool x1, bool x2, bool x3, bool result)
        {
            Build(_ => _
                .Conditional(x1).Conditional(x2).Conditional(x3).Where(x => false)
                .And.Where(x => true)).Should().Be(result);
        }

        public bool Build(
            Func<ILogicOperation<long>, IQueryBuilderResult<long>> builder)
        {
            _ = builder ?? throw new ArgumentException("Builder cannot be null", nameof(builder));

            var init = new QueryBuilder<long>(new OperationStrategy());
            var expression = builder(init).GetExpression();

            return expression.Compile().Invoke(1);
        }
    }
}