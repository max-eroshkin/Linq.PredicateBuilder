namespace Linq.PredicateBuilder.Examples.Models;

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Comment { get; set; }
}

public enum Gender
{
    Male, Female
}

