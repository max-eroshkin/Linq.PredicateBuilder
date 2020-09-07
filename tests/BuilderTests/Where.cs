namespace BuilderTests
{
    using System.Linq;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;
    using static Data.TestData;

    public class Where
    {
        [Fact]
        public void NumberGt()
        {
            var result = DataSet.Build(
                _ => _.Where(x => x.Id > 1));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id > 1));
        }
    }
}