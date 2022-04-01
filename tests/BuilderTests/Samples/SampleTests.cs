namespace BuilderTests.Samples;

using System;
using System.Collections.Generic;
using System.Data;
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
        /*Linq.PredicateBuilder is very useful when you have to fetch data from database using query based on search
         filter parameters. In such cases you usually need to create a lot of boilerplate code to check parameters against
         nulls, empty strings, trim starting and tailing whitespaces before including filtering conditions to query.
         
         Using this library allow you easily create queries using fluent API.*/

        var filter = new Filter
        {
            FirstName = null!,
            LastName = "Brown",
            Gender = Gender.Male,
            Comment = string.Empty,
            Ids = new List<int>()
        };

        // Sample of such query
        var query = Persons.Build(_ => _
            .Equals(x => x.FirstName, filter.FirstName) // FirstName is null -> Ignored
            .And.Equals(x => x.LastName, filter.LastName)
            .And.Equals(x => x.Gender, filter.Gender)
            .And.Contains(x => x.Comment, filter.Comment) // Comment is empty -> Ignored
            .And.In(x => x.Id, filter.Ids)); // Ids is empty -> Ignored

        var lastName = filter.LastName.ToLower();
        var query2 = Persons.Where(x =>
            x.LastName.ToLower().Equals(lastName) && x.Gender.Equals(filter.Gender)).ToList();

        // query and query2 are equal
        query.Should().BeEquivalentTo(query2);

        /*You can combine conditions using logical operators AND and OR*/
        var andQuery = Persons.Build(_ => _
            .Equals(x => x.LastName, filter.LastName)
            .And.Equals(x => x.Gender, filter.Gender));

        var orQuery = Persons.Build(_ => _
            .Contains(x => x.FirstName, filter.FirstName)
            .Or.Contains(x => x.LastName, filter.LastName));

        /*To change the precedence of operations you can use Brackets method with a nested builder*/
        var query3 = Persons.Build(_ => _
            .Contains(x => x.Comment, filter.Comment)
            .And.Brackets(b =>
                b.Equals(x => x.FirstName, filter.FirstName).Or.Equals(x => x.LastName, filter.LastName)));
        var boolean_expression = false;
        var query4 = Persons.Build(_ => _
            .Equals(x => x.LastName, filter.LastName)
            .And.Conditional(boolean_expression).Where(x => x.DateOfBirth < new DateOnly(1990, 1, 1))); // this segment is controlled by .Conditional(boolean_expression)
    }

    /*void HowIgnoringWorks()
    {
        var filter = new
        {
            LastName = "Brown",
            Gender = Gender.Male
        };
        
        var result = Persons.Build(_ => _
            .Equals(x => x.FirstName, filter.FirstName)   // Ignored
            .And.Equals(x => x.LastName, filter.LastName)
            .And.Equals(x => x.Gender, filter.Gender)
            .And.Contains(x => x.Comment, filter.Comment) // Ignored
            .And.In(x => x.Id, filter.Ids));              // Ignored
    }*/
}

public class Filter
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Comment { get; set; }

    public DateTime? Birthdate { get; set; }

    public Gender? Gender { get; set; }

    public List<int> Ids { get; set; }
}