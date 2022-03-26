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
            var result = DataSet.Build(
                _ => _.Contains(x => x.Name, "aaAa"),
                BuilderOptions.None);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void IgnoreCase()
        {
            var result = DataSet.Build(
                _ => _.Contains(x => x.Name, "aaAa"),
                BuilderOptions.IgnoreCase);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 2));
        }

        [Fact]
        public void Empty()
        {
            var result = DataSet.Build(
                _ => _.Contains(x => x.Name, "aaAb"),
                BuilderOptions.IgnoreCase);

            result.Should().BeEmpty();
        }

        [Fact]
        public void TrimInput()
        {
            var result = DataSet.Build(
                _ => _.Contains(x => x.Name, " aaAa1 "),
                BuilderOptions.Trim);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void IgnoreNullInput()
        {
            var result = DataSet.Build(
                _ => _.Contains(x => x.Name, null));

            result.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreEmptyInput()
        {
            var result = DataSet.Build(
                _ => _.Contains(x => x.Name, string.Empty));

            result.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreWhitespaceInput()
        {
            var result = DataSet.Build(
                _ => _.Contains(x => x.Name, " "));

            result.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void UseNullInput()
        {
            Action f = () => DataSet.Build(
                _ => _.Contains(x => x.Name, null),
                BuilderOptions.None);

            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UseDefaultOrEmptyOrWhitespeceInput()
        {
            var result = DataSet.Build(
                _ => _.Contains(x => x.Name, "  "),
                BuilderOptions.None);

            result.Should().BeEmpty();
        }        
        
        [Fact]
        public void CombineAnd()
        {
            var result = DataSet.Build(
                _ => _
                    .Contains(x => x.Name, "A").And
                    .Contains(x => x.Name, "2"),
                BuilderOptions.None);

            var reference = DataSet.Where(x => x.Name.Contains("A") && x.Name.Contains("2"));

            result.Should().BeEquivalentTo(reference);
        }
        
        [Fact]
        public void CombineOr()
        {
            var result = DataSet.Build(
                _ => _
                    .Contains(x => x.Name, "1").Or
                    .Contains(x => x.Name, "2"),
                BuilderOptions.None);

            var reference = DataSet.Where(x => x.Name.Contains("1") || x.Name.Contains("2"));

            result.Should().BeEquivalentTo(reference);
        }
    }
}