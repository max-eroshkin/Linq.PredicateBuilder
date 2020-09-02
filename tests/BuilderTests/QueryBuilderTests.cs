namespace BuilderTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Data;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;

    public class QueryBuilderTests
    {
        [Fact]
        public void Any()
        {
            IOperationStrategy strategy = new OperationStrategy(BuilderOptions.CaseSensitive | BuilderOptions.UseDefaultInputs);

            var exp1 = strategy.Any<Item, Request>(
                x => x.Requests,
                x => x.ApproverId == 3);

            var expression = new QueryBuilder<Item>(strategy).Any(
                    x => x.Requests,
                    i => i.Equals(x => x.ApproverId, 3))
                .GetExpression();

            Expression<Func<Item, bool>> expression2 = x => x.Requests.Any(x => x.ApproverId == 3);
            expression.ToString().Should().Be(expression2.ToString());

            var filtered = TestData.DataSet.FromBuilder(_ => _
                .Any(
                    x => x.Requests,
                    i => i.Equals(x => x.ApproverId, 3))).ToList();
        }

        [Fact]
        public void InTest()
        {
            var set = new List<long> { 1, 2, 3 };
            var ids = new List<long> { 2 };

            var filtered = set.AsQueryable().FromBuilder(_ => _
                .In(x => x, ids)).ToList();
            filtered.Should().BeEquivalentTo(ids);

            var nullFiltered = set.AsQueryable().FromBuilder(_ => _
                .In(x => x, null)).ToList();
            nullFiltered.Should().BeEquivalentTo(set);

            var emptyFiltered = set.AsQueryable().FromBuilder(_ => _
                .In(x => x, Array.Empty<long>())).ToList();
            emptyFiltered.Should().BeEquivalentTo(set);
        }
    }
}