namespace BuilderTests
{
    using System;
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

        [Fact]
        public void CombineAnd()
        {
            var requestIdList = new long[] { 2, 3 };
            var result = DataSet.Build(_ => _
                .Where(_ => true).And
                .Any(p => p.Requests, n => n.In(x => x.Id, requestIdList)));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Requests.Any(n => requestIdList.Contains(n.Id))));
        }

        [Fact]
        public void CombineOr()
        {
            var requestIdList = new long[] { 2, 3 };
            var result = DataSet.Build(_ => _
                .Where(_ => false).Or
                .Any(p => p.Requests, n => n.In(x => x.Id, requestIdList)));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Requests.Any(n => requestIdList.Contains(n.Id))));
        }

        [Fact]
        public void EmptyBuilder()
        {
            Action a = () => DataSet.Build(_ => _.Any(p => p.Requests, null!));
            a.Should().Throw<ArgumentException>().WithMessage("Builder cannot be null (Parameter 'builder')");
        }
    }
}