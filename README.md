# Linq.PredicateBuilder

[![CI](https://github.com/max-eroshkin/Linq.PredicateBuilder/actions/workflows/CI.yml/badge.svg)](https://github.com/max-eroshkin/Linq.PredicateBuilder/actions)
[![Release](https://img.shields.io/nuget/v/Linq.PredicateBuilder?logo=nuget&label=release&color=blue)](https://www.nuget.org/packages/Linq.PredicateBuilder)
[![Latest](https://img.shields.io/nuget/vpre/Linq.PredicateBuilder?logo=nuget&label=latest&color=yellow)](https://www.nuget.org/packages/Linq.PredicateBuilder/absoluteLatest)

This library allows you to construct filtering expressions in run-time using fluent API
and minimize boilerplate code such as null/empty checking and case ignoring.

Linq.PredicateBuilder is very useful when you have to fetch data from database using query based on search
 filter parameters. In such cases you usually need to create a lot of boilerplate code to check parameters against
 nulls, empty strings and trim starting/trailing whitespaces before including filtering conditions in query.
         
 Using this library allow you easily create queries using fluent API.

## Sample
 
 For this sample we will use `Person` class
```c#
public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Comment { get; set; }
}
```
source of Persons
```c#       
IQueryable<Persons> Persons { get; set; }
```

and Filter class instance containing search parameters 
```c#
var filter = new Filter
{
    FirstName = null!,
    LastName = "Brown",
    Gender = Gender.Male,
    Comment = string.Empty,
    Ids = new List<int>()
};
```
Here we build a query from several predicate segments combined together
```c#
var query = Persons.Build(_ => _
    .Equals(x => x.FirstName, filter.FirstName)   // filter.FirstName is null -> this segment will be ignored
    .And.Equals(x => x.LastName, filter.LastName)
    .And.Equals(x => x.Gender, filter.Gender)
    .And.Contains(x => x.Comment, filter.Comment) // filter.Comment is empty -> this segment will be ignored
    .And.In(x => x.Id, filter.Ids));              // filter.Ids is empty -> this segment will be ignored
```
Some of these segments will be ignored because of corresponding search parameters intended to not be use in the query.
This query is equal to the next code:

```c#
var lastName = filter.LastName.ToLower();
var query = Persons.Where(x => x.LastName.ToLower().Equals(lastName) && x.Gender.Equals(filter.Gender));
```
## Expression Combining
You can combine filtering conditions using logical operators _AND_ and _OR_.

```c#
var andQuery = Persons.Build(_ => _
    .Equals(x => x.LastName, filter.LastName)
    .And.Equals(x => x.Gender, filter.Gender));
 ```    
```c#
var orQuery = Persons.Build(_ => _
    .Contains(x => x.FirstName, filter.FirstName)
    .Or.Contains(x => x.LastName, filter.LastName));
  ```      
### Precedence
You can't use _AND_ and _OR_ operators side by side because of there is no easy way to provide precedence of these logical operators.

To mix _ANDs_ and _ORs_ or change the precedence of operators you can use _Brackets_ method with a nested builder
```c#
var query3 = Persons.Build(_ => _
    .Contains(x => x.Comment, filter.Comment)
    .And.Brackets(b => b.Equals(x => x.FirstName, filter.FirstName).Or.Equals(x => x.LastName, filter.LastName)));
```

```c#
var query = Persons.Build(_ => _
    .Equals(x => x.LastName, filter.LastName)
    .And.Conditional(boolean_expression).Equals(x => x.Gender, filter.Gender));
 ```    