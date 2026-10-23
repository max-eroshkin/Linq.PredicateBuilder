# Linq.PredicateBuilder

[![CI](https://github.com/max-eroshkin/Linq.PredicateBuilder/actions/workflows/CI.yml/badge.svg)](https://github.com/max-eroshkin/Linq.PredicateBuilder/actions)
[![Release](https://img.shields.io/nuget/v/Linq.PredicateBuilder?logo=nuget&label=nuget&color=blue)](https://www.nuget.org/packages/Linq.PredicateBuilder)

This library allows you to build dynamic filtering expressions at runtime using a fluent API.

Linq.PredicateBuilder is especially useful when fetching data from a database based on user-defined search filters.
Normally, you would have to write repetitive code to handle null values, empty strings, and whitespace trimming
before applying these conditions to the query.
The library significantly reduces boilerplate code for null/empty checks and case-insensitive comparisons.

## Samples

For this sample, we will use the `Person` entity class and a `Filter` class containing search parameters:
```c#
public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Comment { get; set; }
    public IEnumerable<Person> Relatives { get; set; }
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
Suppose we have a data source of `Person` entities:
```csharp       
IQueryable<Person> Persons { get; }
```

And a `Filter` class instance with the following parameter values:
```csharp
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

Using the fluent API, we can build a query from several predicate segments combined together:
```csharp
var query = Persons.Build(_ => _
    .Equals(x => x.FirstName, filter.FirstName)   // filter.FirstName is null -> this segment will be ignored
    .And.Equals(x => x.LastName, filter.LastName)
    .And.Equals(x => x.Gender, filter.Gender)
    .And.Contains(x => x.Comment, filter.Comment) // filter.Comment is empty -> this segment will be ignored
    .And.In(x => x.Id, filter.Ids)                // filter.Ids is empty -> this segment will be ignored
    .And.Conditional(filter.HasRelatives == true).Where(x => x.Relatives.Any())     // filter.HasRelatives is null -> this segment will be ignored
    .And.Conditional(filter.HasRelatives == false).Where(x => !x.Relatives.Any())); // filter.HasRelatives is null -> this segment will be ignored
```

Some of the builder segments will be ignored automatically because their corresponding search parameters are null or empty.
The generated query is fully equivalent to the following standard LINQ code:

```csharp
var lastName = filter.LastName.ToLower();
var query = Persons.Where(x => x.LastName.ToLower() == lastName && x.Gender == filter.Gender);
```

## Expression Combining
You can combine predicates using the logical operators `And` and `Or`.

```csharp
var query = Persons.Build(_ => _
    .Equals(x => x.LastName, filter.LastName)
    .And.Equals(x => x.Gender, filter.Gender));
```    

```csharp
var query = Persons.Build(_ => _
    .Contains(x => x.FirstName, filter.FirstName)
    .Or.Contains(x => x.LastName, filter.LastName));
```

You can also use the logical negation operator `Not`.
```csharp
var query = Persons.Build(_ => _
    .Not.Equals(x => x.LastName, filter.LastName)
    .And.Not.Contains(x => x.Comment, filter.Comment));
```

### Precedence

To prevent ambiguity, the library does not implicitly define the precedence of logical operators.
Therefore, you cannot chain `And` and `Or` operators consecutively at the same level.

To mix `And` and `Or` operators, or to explicitly change the execution order, use the `Brackets` method with a nested builder:
```c#
var query = Persons.Build(_ => _
    .Contains(x => x.Comment, filter.Comment)
    .And.Brackets(b => b.Equals(x => x.FirstName, filter.FirstName).Or.Equals(x => x.LastName, filter.LastName)));
```

## Ignoring Builder Segments

The builder chain is composed of atomic logical segments connected by operators.
This design allows you to dynamically control query generation based on the provided search filter values.

### Checking Filter Values

Most predicate methods accept two mandatory parameters—a **property selector** and a **filter parameter**—along with an optional **builder options** parameter.

The default options are set to `IgnoreCase | IgnoreDefaultInputs | Trim`, which applies the following behavior:
* **Trim**: If the parameter is a string, its leading and trailing whitespaces are automatically removed.
* **IgnoreCase**: String comparisons are performed in a case-insensitive manner.
* **IgnoreDefaultInputs**: If the parameter value is `default`, `string.Empty`, or an empty collection, the current segment is **ignored** (skipped).

These options can be configured individually per segment or globally for the entire builder.

### `Conditional()`

You can explicitly control whether to include or ignore a segment by placing the `Conditional()` method before it.
If the argument passed to `Conditional()` evaluates to `false`, the subsequent segment will be **ignored** (skipped).

```csharp
var query = Persons.Build(_ => _
    .Equals(x => x.LastName, filter.LastName)
    .And.Conditional(boolean_expression).Where(x => x.DateOfBirth < new DateOnly(1990, 1, 1))); // This segment is controlled by .Conditional(boolean_expression)
```

## Nested Collections

To build predicates for nested collections, you can use the `Any()` method. It accepts a collection selector and a nested builder to define filters for the related entities.

The following example demonstrates this use case:

```csharp
var query = Persons.Build(_ => _
    .Equals(x => x.LastName, filter.LastName)
    .Or.Any(x => x.Relatives, b => b.Equals(r => r.LastName, filter.LastName)));
```

## Builder Predicate Methods

| Method | Description |
|----|----|
| `Equals(selector_expression, input_value)`  | Builds a predicate using the `Equals()` method. |
| `Contains(selector_expression, input_value)`  | Builds a predicate using `String.Contains()`. |
| `In(selector_expression, collection_input_value)` | Builds a predicate using `Enumerable.Contains()` (SQL `IN` clause). |
| `Any(collection_selector_expression, nested_builder)` | Builds a predicate for nested collections. |
| `Where(predicate)` | Directly applies the provided predicate to the query. |
| `Where(predicate, input_value)` | A conditional `Where` method that validates the input value before applying. |

The input value of the conditional `Where` method is passed directly into the predicate. The library validates this value against the active `BuilderOptions` (e.g., checking for `null` or `default`) and automatically ignores the segment if the condition is not met:

```csharp
.Where((x, parameter) => x.DateOfBirth >= parameter, filter.DateOfBirth)
```

## Building Predicates

In the examples above, queries were created directly using the `Build` extension method. If you only need to generate the predicate expression itself (for example, to reuse it later), use the static `QueryableBuilderExtensions.BuildPredicate` method:

```csharp
var predicate = QueryableBuilderExtensions.BuildPredicate<Person>(_ => _
    .Equals(x => x.LastName, filter.LastName)
    .Or.Any(x => x.Relatives, b => b.Equals(r => r.LastName, filter.LastName)));

// If all segments are ignored, BuildPredicate returns null. 
// You can use a fallback expression like this:
var query = Persons.Where(predicate ?? (x => true));
```
