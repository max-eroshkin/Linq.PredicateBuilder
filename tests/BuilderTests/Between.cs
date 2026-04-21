// Copyright (c) 2023-2026 Platforma. All rights reserved.

namespace BuilderTests;

using System.Linq;
using FluentAssertions;
using Linq.PredicateBuilder;
using Xunit;
using static Data.TestData;

public class BetweenTests
{
    [Fact]
    public void Between()
    {
        var query = DataSet.Build(_ => _.Between(x => x.Id, 1, 3));

        query.Should().BeEquivalentTo(
            DataSet.Where(x => x.Id >= 1 && x.Id <= 3));
    }

    [Fact]
    public void BetweenSwappedBoundaries()
    {
        var query = DataSet.Build(_ => _.Between(x => x.Id, 3, 1));

        query.Should().BeEquivalentTo(
            DataSet.Where(x => x.Id >= 1 && x.Id <= 3));
    }

    [Fact]
    public void BetweenIgnoringIfAnyNull()
    {
        var query = DataSet.Build(_ => _.Between(x => x.Id, null, 3));

        query.Should().BeEquivalentTo(DataSet);
    }

    [Fact]
    public void BetweenIgnoringIfBothNull()
    {
        var query = DataSet.Build(_ => _.Between(x => x.Id, null, null, false));

        query.Should().BeEquivalentTo(DataSet);
    }

    [Fact]
    public void BetweenNotIgnoringIfSingleNull()
    {
        var query = DataSet.Build(_ => _.Between(x => x.Id, null, 3, false));

        query.Should().BeEquivalentTo(
            DataSet.Where(x => x.Id <= 3));
    }
}
