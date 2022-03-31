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

##Sample
      
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
        
Sample of such query:
```c#
var query = Persons.Build(_ => _
    .Equals(x => x.FirstName, filter.FirstName)   // FirstName is null -> Ignored
    .And.Equals(x => x.LastName, filter.LastName)
    .And.Equals(x => x.Gender, filter.Gender)
    .And.Contains(x => x.Comment, filter.Comment) // Comment is empty -> Ignored
    .And.In(x => x.Id, filter.Ids));              // Ids is empty -> Ignored
```

This query is equal to the next code:

```c#
var lastName = filter.LastName.ToLower();
var query = Persons.Where(x => x.LastName.ToLower().Equals(lastName) && x.Gender.Equals(filter.Gender));
```
##Expression Combining
You can combine conditions using logical operators _AND_ and _OR_.

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
###Precedence
To change the precedence of operations you can use _Brackets_ method with a nested builder
```c#
var query3 = Persons.Build(_ => _
    .Contains(x => x.Comment, filter.Comment)
    .And.Brackets(b => b.Equals(x => x.FirstName, filter.FirstName).Or.Equals(x => x.LastName, filter.LastName)));
```