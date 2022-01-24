namespace BuilderTests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;
    using static Data.TestData;

    public class ConditionalWhere
    {
        [Fact]
        public void CaseSensitive()
        {
            var result = DataSet.Build(
                _ => _.Where((x, input) => x.Name == input, "aaAa1"),
                BuilderOptions.None);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void CaseSensitiveEmpty()
        {
            var result = DataSet.Build(
                _ => _.Where((x, input) => x.Name == input, "aAAa1"),
                BuilderOptions.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public void IgnoreCase()
        {
            var result = DataSet.Build(
                _ => _.Where((x, input) => x.Name.ToLower() == input, "aAAa1"),
                BuilderOptions.IgnoreCase);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void IgnoreCaseEmpty()
        {
            var result = DataSet.Build(
                _ => _.Where((x, input) => x.Name.ToLower() == input, "aaAb3"),
                BuilderOptions.IgnoreCase);

            result.Should().BeEmpty();
        }

        [Fact]
        public void TrimInput()
        {
            var result = DataSet.Build(
                _ => _.Where((x, input) => x.Name == input, " aaAa1 "),
                BuilderOptions.Trim);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1));
        }

        [Fact]
        public void NonString()
        {
            var result = DataSet.Build(
                _ => _.Where((x, input) => x.ParentId == input, 1));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 3));
        }

        [Fact]
        public void IgnoreNullStringInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Where((x, input) => x.Name == input, (string?)null));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreNullLongInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Where((x, input) => x.ParentId == input, (long?)null));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreEmptyInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Where((x, input) => x.Name == input, string.Empty));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreWhitespaceInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Where((x, input) => x.Name == input, " "));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreEmptyEnumerableInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Where((x, input) => input.Contains(x.Name), Array.Empty<string>()));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void EnumerableInput()
        {
            var set = new[] { "aaAa1", "baBa" };
            var resultNull = DataSet.Build(
                _ => _.Where((x, input) => input.Contains(x.Name), set));

            resultNull.Should().BeEquivalentTo(DataSet.AsEnumerable().Where(x => set.Contains(x.Name)));
        }

        [Fact]
        public void UseDefaultOrEmptyOrWhitespaceInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Where((x, input) => x.Name == input, (string?)null),
                BuilderOptions.None);

            resultNull.Should().BeEmpty();
        }

        [Fact]
        public void UseDefaultNonStringInput()
        {
            var resultNull = DataSet.Build(
                _ => _.Where((x, input) => x.ParentId == input, (long?)null),
                BuilderOptions.None);

            resultNull.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 2));
        }

        [Fact]
        public void CombineAnd()
        {
            var resultNull = DataSet.Build(
                _ => _
                    .Where(_ => true).And
                    .Where((x, input) => x.ParentId == input, (long?)null),
                BuilderOptions.None);

            resultNull.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 2));
        }

        [Fact]
        public void CombineOr()
        {
            var resultNull = DataSet.Build(
                _ => _
                    .Where(_ => false).Or
                    .Where((x, input) => x.ParentId == input, (long?)null),
                BuilderOptions.None);

            resultNull.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 2));
        }
    }
}