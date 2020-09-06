namespace BuilderTests
{
    using System.Linq;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;
    using static Data.TestData;

    public class Combine
    {
        [Fact]
        void IgnoreCase()
        {
            var result = DataSet.FromBuilder(
                _ => _.Where(x => x.Id > 1)
                    .And.Contains(x => x.Name, "BAB"))
                .ToList();

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id > 1 && x.Name.ToLower().Contains("bab")));
        }

        [Fact]
        void IgnoreCaseIgnoreDefault()
        {
            var result = DataSet.FromBuilder(
                _ => _.Where(x => x.Id > 1)
                    .And.Contains(x => x.Name, null))
                .ToList();

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id > 1));
        }
    }
}