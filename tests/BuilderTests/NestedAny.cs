namespace BuilderTests
{
    using System.Linq;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;
    using static Data.TestData;

    public class NestedAny
    {
        [Fact]
        public void NestedIn()
        {
            var requestIdList = new long[] { 2, 3 };
            var result = DataSet.Build(
                _ => _.Any(p => p.Requests, n => n.In(x => x.Id, requestIdList)));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Requests.Any(n => requestIdList.Contains(n.Id))));
        }

        [Fact]
        public void NestedInEmpty()
        {
            var requestIdList = new long[] { -2, -1 };
            var result = DataSet.Build(
                _ => _.Any(p => p.Requests, n => n.In(x => x.Id, requestIdList)));

            result.Should().BeEmpty();
        }

        [Fact]
        public void IgnoreDefault()
        {
            long[]? requestIdList = null;
            var result = DataSet.Build(
                _ => _.Any(p => p.Requests, n => n.In(x => x.Id, requestIdList)));

            result.Should().BeEquivalentTo(DataSet);
        }
    }
}