namespace Linq.PredicateBuilder.Examples.Models;

public record Person(int Id, string FirstName, string LastName, DateTime? BirthDate, Gender Gender, string Comment = "");

public enum Gender
{
    Male, Female
}

