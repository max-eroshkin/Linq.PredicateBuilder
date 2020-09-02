namespace BuilderTests
{
    using System.Linq;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;
    using static Data.TestData;

    public class Contains
    {
        [Fact]
        public void CaseSensitive()
        {
            var result = DataSet.FromBuilder(
                    _ => _.Contains(x => x.Name, "aaAa"),
                    BuilderOptions.CaseSensitive);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void IgnoreCase()
        {
            var result = DataSet.FromBuilder(
                    _ => _.Contains(x => x.Name, "aaAa"));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 2));
        }

        [Fact]
        public void Empty()
        {
            var result = DataSet.FromBuilder(
                    _ => _.Contains(x => x.Name, "aaAb"),
                    BuilderOptions.CaseSensitive);

            result.Should().BeEmpty();
        }

        [Fact]
        public void TrimInput()
        {
            var result = DataSet.FromBuilder(
                _ => _.Contains(x => x.Name, " aaAa1 "),
                BuilderOptions.CaseSensitive);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void IgnoreNullInput()
        {
            var resultNull = DataSet.FromBuilder(
                    _ => _.Contains(x => x.Name, null));

            resultNull.Should().BeEquivalentTo(DataSet);
        }
 
        [Fact]
        public void IgnoreEmptyInput()
        {
            var resultNull = DataSet.FromBuilder(
                    _ => _.Contains(x => x.Name, string.Empty));

            resultNull.Should().BeEquivalentTo(DataSet);
        }
 
        [Fact]
        public void IgnoreWhitespaceInput()
        {
            var resultNull = DataSet.FromBuilder(
                    _ => _.Contains(x => x.Name, " "));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void UseDefaultOrEmptyOrWhitespeceInput()
        {
            var resultNull = DataSet.FromBuilder(
                _ => _.Contains(x => x.Name, null),
                BuilderOptions.UseDefaultInputs);

            resultNull.Should().BeEmpty();
        }
    }
}