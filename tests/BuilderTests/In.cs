﻿namespace BuilderTests
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
            var result = DataSet.Build(
                _ => _.In(x => x.Name, new[] { "aaAa1", "baBa" }),
                BuilderOptions.None);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 3));
        }

        [Fact]
        public void CaseSensitiveEmpty()
        {
            var result = DataSet.Build(
                _ => _.In(x => x.Name, new[] { " baBa" }),
                BuilderOptions.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public void IgnoreCase()
        {
            var result = DataSet.Build(
                _ => _.In(x => x.Name, new[] { "aaAa1", "baBa" }),
                BuilderOptions.IgnoreCase).ToList();

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 3 || x.Id == 4));
        }

        [Fact]
        public void IgnoreCaseEmpty()
        {
            var result = DataSet.Build(
                _ => _.In(x => x.Name, new[] { "baBae" }),
                BuilderOptions.IgnoreCase);

            result.Should().BeEmpty();
        }

        [Fact]
        public void TrimInput()
        {
            var result = DataSet.Build(
                _ => _.In(x => x.Name, new[] { " aaAa1", "baBa  " }),
                BuilderOptions.Trim);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 3));
        }

        [Fact]
        public void NonString()
        {
            var result = DataSet.Build(
                _ => _.In(x => x.ParentId, new long?[] { 1 }));

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 3));
        }

        [Fact]
        public void IgnoreNullStringInput()
        {
            var resultNull = DataSet.Build(
                _ => _.In(x => x.Name, null));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void IgnoreNullLongInput()
        {
            var resultNull = DataSet.Build(
                _ => _.In(x => x.ParentId, null));

            resultNull.Should().BeEquivalentTo(DataSet);
        }

        [Fact]
        public void UseNullInput()
        {
            Action f = () => DataSet.Build(
                _ => _.In(x => x.Name, null),
                BuilderOptions.None);

            f.Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void CombineAnd()
        {
            var result = DataSet.Build(
                _ => _
                    .Where(_ => true).And
                    .In(x => x.Name, new[] { "aaAa1", "baBa" }),
                BuilderOptions.None);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 3));
        } 
        
        [Fact]
        public void CombineOr()
        {
            var result = DataSet.Build(
                _ => _
                    .Where(_ => false).Or
                    .In(x => x.Name, new[] { "aaAa1", "baBa" }),
                BuilderOptions.None);

            result.Should().BeEquivalentTo(DataSet.Where(x => x.Id == 1 || x.Id == 3));
        }
    }
}