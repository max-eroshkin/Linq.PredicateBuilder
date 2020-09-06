namespace BuilderTests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Linq.PredicateBuilder;
    using Xunit;
    using static Data.TestData;

    public class In
    {
        [Fact]
        public void CaseSensitive()
        {
            var result = DataSet.FromBuilder(
                _ => _.In(x => x.Name, new[] { "aaAa1", "baBa" }),
                BuilderOptions.None);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 3));
        }

        [Fact]
        public void CaseSensitiveEmpty()
        {
            var result = DataSet.FromBuilder(
                _ => _.In(x => x.Name, new[] { " baBa" }),
                BuilderOptions.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public void IgnoreCase()
        {
            var result = DataSet.FromBuilder(
                _ => _.In(x => x.Name, new[] { "aaAa1", "baBa" }),
                BuilderOptions.IgnoreCase).ToList();

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 3 || x.Id == 4));
        }

        [Fact]
        public void IgnoreCaseEmpty()
        {
            var result = DataSet.FromBuilder(
                _ => _.In(x => x.Name, new[] { "baBae" }),
                BuilderOptions.IgnoreCase);

            result.Should().BeEmpty();
        }

        [Fact]
        public void TrimInput()
        {
            var result = DataSet.FromBuilder(
                _ => _.In(x => x.Name, new[] { " aaAa1", "baBa  " }),
                BuilderOptions.Trim);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 3));
        }

        [Fact]
        public void NonString()
        {
            var result = DataSet.FromBuilder(
                _ => _.In(x => x.ParentId, new long?[] { 1 }));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 3));
        }

        [Fact]
        public void IgnoreNullStringInput()
        {
            var resultNull = DataSet.FromBuilder(
                _ => _.In(x => x.Name, null));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreNullLongInput()
        {
            var resultNull = DataSet.FromBuilder(
                _ => _.In(x => x.ParentId, null));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void UseNullInput()
        {
            Action f = () => DataSet.FromBuilder(
                _ => _.In(x => x.Name, null),
                BuilderOptions.None);

            f.Should().Throw<ArgumentNullException>();
        }
    }
}