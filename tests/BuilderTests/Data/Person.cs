namespace BuilderTests.Data;

using System;

public record Person(int Id, string FirstName, string LastName, DateOnly? DateOfBirth, Gender Gender, string Comment = "");

public enum Gender
{
    Male, 
    Female
}

