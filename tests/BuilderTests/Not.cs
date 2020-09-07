namespace BuilderTests
{
    using System.Linq;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;
    using static Data.TestData;

    public class Not
    {
        [Fact]
        public void NumberGt()
        {
            var result = DataSet.Build(
                _ => _.Not.Where(x => x.Id > 1));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id <= 1));
        }

        [Fact]
        public void StringEquals()
        {
            var result = DataSet.Build(
                _ => _.Not.Equals(x => x.Name, "aaAa1"));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Name.ToLower() != "aaaa1"));
        }

        [Fact]
        public void Contains()
        {
            var result = DataSet.Build(
                _ => _.Not.Contains(x => x.Name, "b"));

            result.Should().BeEquivalentTo(DataSet.Where(x => !x.Name.ToLower().Contains("b")));
        }
    }
}