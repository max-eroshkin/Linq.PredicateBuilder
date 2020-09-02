namespace BuilderTests
{
    using System;
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
                    BuilderOptions.None);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void IgnoreCase()
        {
            var result = DataSet.FromBuilder(
                    _ => _.Contains(x => x.Name, "aaAa"),
                    BuilderOptions.IgnoreCase);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 2));
        }

        [Fact]
        public void Empty()
        {
            var result = DataSet.FromBuilder(
                    _ => _.Contains(x => x.Name, "aaAb"),
                    BuilderOptions.IgnoreCase);

            result.Should().BeEmpty();
        }

        [Fact]
        public void TrimInput()
        {
            var result = DataSet.FromBuilder(
                _ => _.Contains(x => x.Name, " aaAa1 "),
                BuilderOptions.Trim);

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
        public void UseNullInput()
        {
            Action f = () => DataSet.FromBuilder(
                _ => _.Contains(x => x.Name, null),
                BuilderOptions.None);

            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UseDefaultOrEmptyOrWhitespeceInput()
        {
            var resultNull = DataSet.FromBuilder(
                _ => _.Contains(x => x.Name, "  "),
                BuilderOptions.None);

            resultNull.Should().BeEmpty();
        }
    }
}