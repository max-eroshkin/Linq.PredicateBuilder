# Linq.PredicateBuilder

[![CI](https://github.com/max-eroshkin/Linq.PredicateBuilder/actions/workflows/CI.yml/badge.svg)](https://github.com/max-eroshkin/Linq.PredicateBuilder/actions)
[![Release](https://img.shields.io/nuget/v/Linq.PredicateBuilder?logo=nuget&label=nuget&color=blue)](https://www.nuget.org/packages/Linq.PredicateBuilder)

This library allows you to construct filtering expressions at run-time on the fly using fluent API
and minimize boilerplate code such as null/empty checking and case ignoring.

Linq.PredicateBuilder can be useful when you have to fetch data from database using query based on search
filter parameters. In such cases you usually need to create a lot of boilerplate code to check parameters against
nulls, empty strings and trim starting/trailing whitespaces before including filtering conditions in query.

## Samples

For this sample we will use `Person` entity class and `Filter` class containing search parameters
```c#
public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Comment { get; set; }
    public IEnumerable<Person> Relatives{ get; set; }
}

public class Filter
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Comment { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public List<int> Ids { get; set; }
    public bool? HasRelatives { get; set; }
}
```
We have a source of Persons
```c#       
IQueryable<Persons> Persons { get; }
```

and `Filter` class instance with parameters values
```c#
var filter = new Filter
{
    FirstName = null,
    LastName = "Brown",
    Gender = Gender.Male,
    Comment = string.Empty,
    Ids = new List<int>(),
    HasRelatives = null
};
```
So we can build a query from several predicate segments combined together.
```c#
var query = Persons.Build(_ => _
    .Equals(x => x.FirstName, filter.FirstName)   // filter.FirstName is null -> this segment will be ignored
    .And.Equals(x => x.LastName, filter.LastName)
    .And.Equals(x => x.Gender, filter.Gender)
    .And.Contains(x => x.Comment, filter.Comment) // filter.Comment is empty -> this segment will be ignored
    .And.In(x => x.Id, filter.Ids)                // filter.Ids is empty -> this segment will be ignored
    .And.Conditional(filter.HasRelatives == true).Where(x => x.Relatives.Any())     // filter.HasRelatives is null -> this segment will be ignored
    .And.Conditional(filter.HasRelatives == false).Where(x => !x.Relatives.Any())); // filter.HasRelatives is null -> this segment will be ignored
```
Some of builder segments will be ignored because corresponding search parameters should not be used in the query.
This query is equal to the following code:

```c#
var lastName = filter.LastName.ToLower();
var query = Persons.Where(x => x.LastName.ToLower().Equals(lastName) && x.Gender.Equals(filter.Gender));
```
## Expression Combining
You can combine predicates using logical operators _AND_ and _OR_.

```c#
var query = Persons.Build(_ => _
    .Equals(x => x.LastName, filter.LastName)
    .And.Equals(x => x.Gender, filter.Gender));
 ```    
```c#
var query = Persons.Build(_ => _
    .Contains(x => x.FirstName, filter.FirstName)
    .Or.Contains(x => x.LastName, filter.LastName));
```
You can also use logical negation operator _NOT_.
```c#
var query = Persons.Build(_ => _
    .Not.Equals(x => x.LastName, filter.LastName)
    .And.Not.Contains(x => x.Comment, filter.Comment));
```
### Precedence
You can't use _AND_ and _OR_ operators side by side because the precedence of those operators is not provided.

To mix _ANDs_ and _ORs_ or change the precedence of operators you can use `Brackets` method with a nested builder.
```c#
var query = Persons.Build(_ => _
    .Contains(x => x.Comment, filter.Comment)
    .And.Brackets(b => b.Equals(x => x.FirstName, filter.FirstName).Or.Equals(x => x.LastName, filter.LastName)));
```
## Ignoring Builder Segments
As you can see in the samples above, builder chain is divided into atomic logical segments connected with operators.
Let's see how you can control query building depending on search filter parameter values.

### Checking Filter Values
Most of predicate methods have two mandatory parameters: _property selector_, _filter parameter_ and one optional _builder options_.
The default _builder options_ equal to `IgnoreCase | IgnoreDefaultInputs | Trim`, which means that:
- if parameter type is string, the starting and trailing whitespaces will be removed from its value
- string operation will be performed case insensitive
- if parameter value equals to `default`, `string.Empty` or empty collection then current segment will be **ignored** (skipped).

Builder parameters can be changed per segment or for whole builder.

### Conditional()
You can control whether to ignore segment or not using `Conditional` method before the segment. If parameter of
`Conditional()` evaluates to `false` the segment will be **ignored** (skipped).
```c#
var query = Persons.Build(_ => _
    .Equals(x => x.LastName, filter.LastName)
    .And.Conditional(boolean_expression).Where(x => x.DateOfBirth < new DateOnly(1990, 1, 1))); // this segment is controlled by .Conditional(boolean_expression)
 ```    

## Nested Collections
To build predicates for nested collection you have `Any` method that has collection selector and nested builder
for the collection. Following code shows the use case
```c#
var query = Persons.Build(_ => _
    .Equals(x => x.LastName, filter.LastName)
    .Or.Any(x => x.Relatives, b => b.Equals(x => x.LastName, filter.LastName)));
```

## Builder Predicate Methods
| Method                                                | Description                                         |
|-------------------------------------------------------|-----------------------------------------------------|
| `Equals(selector_expression, input_value)`            | builds a predicate based on `Equals()`              |
| `Contains(selector_expression, input_value)`          | builds a predicate based on `String.Contains()`     |
| `In(selector_expression, collection_input_value)`     | builds a predicate based on `Enumerable.Contains()` |
| `Any(collection_selector_expression, nested_builder)` | builds a predicate for nested collections           |
| `Where(predicate)`                                    | uses the predicate from its parameter               |
| `Where(predicate, input_value)`                       | _conditional_ `Where()` method                      |

The input value of _conditional_ `Where` method is being passed into the predicate as a parameter,
and being checked against conditions to ignore the segment if needed:
```c#
.Where((x, parameter) => x.DateOfBirth >= parameter, filter.DateOfBirth)
```

## Building Predicates
In the samples above we created queries using `Build` extension method. To build a predicate expression use
static `QueryableBuilderExtensions.BuildPredicate` method.
```c#
var predicate = QueryableBuilderExtensions.BuildPredicate<Person>(_ => _
    .Equals(x => x.LastName, filter.LastName)
    .Or.Any(x => x.Relatives, b => b.Equals(x => x.LastName, filter.LastName)));

var query = Persons.Where(predicate ?? (x => true));
```