namespace Linq.PredicateBuilder.Examples;

using Models;
using Linq.PredicateBuilder;

public class Example
{
    public Example()
    {
        Persons = new Person[]
        {
            new(1, "John", "Smith", new DateTime(1977, 5, 13), Gender.Male),
            new(2, "Barbara", "Johnson", new DateTime(1985, 1, 1), Gender.Male),
            new(3, "Michael", "Brown", new DateTime(1997, 11, 20), Gender.Male),
            new(4, "Charles", "Walker", new DateTime(1981, 8, 18), Gender.Male),
            new(5, "Maria", "Carter", new DateTime(2000, 12, 7), Gender.Male),
            new(6, "Catherine", "Walker", new DateTime(1987, 5, 3), Gender.Male),
            new(7, "Diane", "Brown", new DateTime(2001, 2, 3), Gender.Male),
            new(8, "Catherine", "Campbell", new DateTime(2003, 4, 3), Gender.Male),
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