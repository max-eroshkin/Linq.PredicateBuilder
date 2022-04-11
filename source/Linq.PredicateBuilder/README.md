# Linq.PredicateBuilder
This library allows you to construct filtering expressions at run-time on the fly using fluent API
and minimize boilerplate code such as null/empty checking and case ignoring.

Linq.PredicateBuilder can be useful when you have to fetch data from database using query based on search
filter parameters. In such cases you usually need to create a lot of boilerplate code to check parameters against
nulls, empty strings and trim starting/trailing whitespaces before including filtering conditions in query.

## Sample
```c#
var query = Persons.Build(_ => _
    .Equals(x => x.FirstName, filter.FirstName)
    .And.Equals(x => x.LastName, filter.LastName)
    .And.Equals(x => x.Gender, filter.Gender)
    .And.Contains(x => x.Comment, filter.Comment)
    .And.In(x => x.Id, filter.Ids)
    .And.Conditional(filter.HasRelatives == true).Where(x => x.Relatives.Any())
    .And.Conditional(filter.HasRelatives == false).Where(x => !x.Relatives.Any()));
```
## Documentation
- [Project site on GitHub](https://github.com/max-eroshkin/Linq.PredicateBuilder)