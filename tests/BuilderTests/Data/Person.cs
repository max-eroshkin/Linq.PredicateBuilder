namespace BuilderTests.Data;

using System;
using System.Collections;
using System.Collections.Generic;

////public record Person(int Id, string FirstName, string LastName, DateOnly? DateOfBirth, Gender Gender, string Comment = "", IEnumerable<Person> Relatives = null!);

public class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public string Comment { get; set; } = string.Empty;

    public IEnumerable<Person> Relatives { get; set; } = Array.Empty<Person>();
}

public enum Gender
{
    Male,
    Female
}