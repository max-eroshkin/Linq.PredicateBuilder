namespace BuilderTests
{
    using System.Linq;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;
    using static Data.TestData;

    public class Equals
    {
        [Fact]
        public void CaseSensitive()
        {
            var result = DataSet.Build(
                _ => _.Equals(x => x.Name, "aaAa1"),
                BuilderOptions.None);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void CaseSensitiveEmpty()
        {
            var result = DataSet.Build(
                _ => _.Equals(x => x.Name, "aAAa1"),
                BuilderOptions.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public void IgnoreCase()
        {
            var result = DataSet.Build(
                _ => _.Equals(x => x.Name, "aAAa1"),
                BuilderOptions.IgnoreCase);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void IgnoreCaseEmpty()
        {
            var result = DataSet.Build(
                _ => _.Equals(x => x.Name, "aaAb3"),
                BuilderOptions.IgnoreCase);

            result.Should().BeEmpty();
        }

        [Fact]
        public void TrimInput()
        {
            var result = DataSet.Build(
                _ => _.Equals(x => x.Name, " aaAa1 "),
                BuilderOptions.Trim);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void NonString()
        {
            var result = DataSet.Build(
                _ => _.Equals(x => x.ParentId, 1));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 3));
        }

        [Fact]
        public void IgnoreNullStringInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Equals(x => x.Name, null));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreNullLongInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Equals(x => x.ParentId, null));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreEmptyInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Equals(x => x.Name, string.Empty));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreWhitespaceInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Equals(x => x.Name, " "));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void UseDefaultOrEmptyOrWhitespaceInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Equals(x => x.Name, null),
                BuilderOptions.None);

            resultNull.Should().BeEmpty();
        }

        [Fact]
        public void UseDefaultNonStringInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Equals(x => x.ParentId, null),
                BuilderOptions.None);

            resultNull.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 2));
        }
    }
}