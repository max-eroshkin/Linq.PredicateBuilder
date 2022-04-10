namespace BuilderTests.Samples;

using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using FluentAssertions;
using Linq.PredicateBuilder;
using Xunit;

public class SampleTests
{
    private IQueryable<Person> Persons => TestData.Persons;

    [Fact]
    public void Ignoring()
    {
        var filter = new Filter
        {
            FirstName = null!,
            LastName = "Brown",
            Gender = Gender.Male,
            Comment = string.Empty,
            Ids = new List<int>()
        };

        var query = Persons.Build(_ => _
            .Equals(x => x.FirstName, filter.FirstName) // FirstName is null -> Ignored
            .And.Equals(x => x.LastName, filter.LastName)
            .And.Equals(x => x.Gender, filter.Gender)
            .And.Contains(x => x.Comment, filter.Comment) // Comment is empty -> Ignored
            .And.In(x => x.Id, filter.Ids) // Ids is empty -> Ignored
            .And.Conditional(filter.HasRelatives == true).Where(x => x.Relatives.Any()) // Ignored
            .And.Conditional(filter.HasRelatives == false).Where(x => !x.Relatives.Any())); // Ignored

        var lastName = filter.LastName.Trim().ToLower();
        query.Should().BeEquivalentTo(
            Persons.Where(x => x.LastName.ToLower().Equals(lastName) && x.Gender.Equals(filter.Gender)));
    }

    [Fact]
    public void Combining()
    {
        var filter = new Filter
        {
            FirstName = "Charles",
            LastName = "Brown",
            Gender = Gender.Male,
        };

        var andQuery = Persons.Build(_ => _
            .Equals(x => x.LastName, filter.LastName)
            .And.Equals(x => x.Gender, filter.Gender));

        var orQuery = Persons.Build(_ => _
            .Contains(x => x.FirstName, filter.FirstName)
            .Or.Contains(x => x.LastName, filter.LastName));

        var lastName = filter.LastName.Trim().ToLower();
        var firstName = filter.FirstName.Trim().ToLower();

        andQuery.Should().BeEquivalentTo(
            Persons.Where(x =>
                x.LastName.ToLower().Equals(lastName) && x.Gender == filter.Gender));

        orQuery.Should().BeEquivalentTo(
            Persons.Where(x =>
                x.FirstName.ToLower().Equals(firstName) || x.LastName.ToLower().Equals(lastName)));
    }

    [Fact]
    public void Precedence()
    {
        var filter = new Filter
        {
            FirstName = "Charles ",
            LastName = "Carter",
            Comment = "this",
        };

        var query = Persons.Build(_ => _
            .Contains(x => x.Comment, filter.Comment)
            .And.Brackets(b =>
                b.Equals(x => x.FirstName, filter.FirstName).Or.Equals(x => x.LastName, filter.LastName)));

        var lastName = filter.LastName.Trim().ToLower();
        var firstName = filter.FirstName.Trim().ToLower();
        var comment = filter.Comment.Trim().ToLower();

        query.Should().BeEquivalentTo(
            Persons.Where(x =>
                x.Comment.ToLower().Contains(comment) &&
                (x.FirstName.ToLower().Equals(firstName) || x.LastName.ToLower().Equals(lastName))));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Conditional(bool boolean)
    {
        var filter = new Filter
        {
            LastName = "brown",
            DateOfBirth = new DateOnly(1999, 1, 1)
        };

        var query = Persons.Build(_ => _
            .Equals(x => x.LastName, filter.LastName)
            .And.Conditional(boolean).Where(x => x.DateOfBirth < filter.DateOfBirth));

        var lastName = filter.LastName.Trim().ToLower();

        if (boolean)
        {
            query.Should().BeEquivalentTo(
                Persons.Where(x =>
                    x.LastName.ToLower().Equals(lastName) && x.DateOfBirth < filter.DateOfBirth));
        }
        else
        {
            query.Should().BeEquivalentTo(
                Persons.Where(x =>
                    x.LastName.ToLower().Equals(lastName)));
        }
    }

    [Fact]
    public void Nested()
    {
        var filter = new Filter
        {
            LastName = "walker",
        };
        var query = Persons.Build(_ => _
            .Equals(x => x.LastName, filter.LastName)
            .Or.Any(x => x.Relatives, b => b.Equals(x => x.LastName, filter.LastName)));

        var lastName = filter.LastName.Trim().ToLower();

        query.Should().BeEquivalentTo(Persons.Where(x =>
            x.LastName.ToLower().Equals(lastName) ||
            x.Relatives.Any(r => r.LastName.ToLower().Equals(lastName))));
    }
}