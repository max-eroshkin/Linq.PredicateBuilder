namespace Linq.PredicateBuilder.Examples;

using Models;
using Linq.PredicateBuilder;

public class Example
{
    public Example()
    {
        Persons = new Person[]
        {
            new() { Id = 1, FirstName = "John", LastName = "Smith", BirthDate = new DateTime(1977, 5, 13), Gender = Gender.Male},
            new() { Id = 2, FirstName = "Barbara", LastName = "Johnson", BirthDate = new DateTime(1985, 1, 1), Gender = Gender.Male },
            new() { Id = 3, FirstName = "Michael", LastName = "Brown", BirthDate = new DateTime(1997, 11, 20), Gender = Gender.Male },
            new() { Id = 4, FirstName = "Charles", LastName = "Walker", BirthDate = new DateTime(1981, 8, 18), Gender = Gender.Male },
            new() { Id = 5, FirstName = "Maria", LastName = "Carter", BirthDate = new DateTime(2000, 12, 7), Gender = Gender.Male },
            new() { Id = 6, FirstName = "Catherine", LastName = "Walker", BirthDate = new DateTime(1987, 5, 3), Gender = Gender.Male },
            new() { Id = 7, FirstName = "Diane", LastName = "Brown", BirthDate = new DateTime(2001, 2, 3), Gender = Gender.Male },
            new() { Id = 8, FirstName = "Catherine", LastName = "Campbell", BirthDate = new DateTime(2003, 4, 3), Gender = Gender.Male },
        }.AsQueryable();
    }

    IQueryable<Person> Persons { get; }

    void Filtering()
    {
        var filter = new
        {
            FirstName = (string)null,
            LastName = "Brown",
            Gender = Gender.Male,
            Comment = "",
            Ids = new List<int>()
        };
        
        var query = Persons.Build(_ => _
            .Equals(x => x.FirstName, filter.FirstName)   // FirstName is null -> Ignored
            .And.Equals(x => x.LastName, filter.LastName)
            .And.Equals(x => x.Gender, filter.Gender)
            .And.Contains(x => x.Comment, filter.Comment) // Comment is empty -> Ignored
            .And.In(x => x.Id, filter.Ids));              // Ids is empty -> Ignored

        var lastName = filter.LastName.ToLower();
        // query and query2 are equal
        var query2 = Persons.Where(x => 
            x.LastName.ToLower().Equals(lastName) &&
            x.Gender.Equals(filter.Gender));
    }
    
    void HowIgnoringWorks()
    {
        //
        var filter = new Filter
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
    }
    
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