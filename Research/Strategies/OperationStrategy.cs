namespace Research;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

/// <inheritdoc />
public class OperationStrategy : IOperationStrategy
{
    private static readonly MethodInfo ToLowerMethod = typeof(string).GetMethods()
        .First(x => x.Name == nameof(string.ToLower) && x.ToString() == "System.String ToLower()");

    private static readonly MethodInfo ContainsMethod =
        typeof(string)
            .GetMethods()
            .First(x => x.Name == nameof(string.Contains) && x.ToString() == "Boolean Contains(System.String)");

    private static readonly MethodInfo CollectionContainsMethod =
        typeof(Enumerable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(x => x.Name == "Contains" && x.GetParameters().Length == 2);

    private static readonly MethodInfo AnyMethod =
        typeof(Enumerable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(x => x.Name == "Any" && x.GetParameters().Length == 2);

    private readonly BuilderOptions _options;

    /// <summary>
    /// Constructs a strategy,
    /// </summary>
    /// <param name="options">A filtering options</param>
    public OperationStrategy(BuilderOptions options = BuilderOptions.Default)
    {
        _options = options;
    }

    private bool IgnoreDefaults => (_options & BuilderOptions.IgnoreDefaultInputs) != 0;

    private bool IgnoreCase => (_options & BuilderOptions.IgnoreCase) != 0;

    private bool TrimStrings => (_options & BuilderOptions.Trim) != 0;

    /// <inheritdoc />
    public Expression<Func<TEntity, bool>> Contains<TEntity>(
        Expression<Func<TEntity, string>> propertyExpression,
        string? input)
    {
        if (IgnoreDefaults && string.IsNullOrWhiteSpace(input))
            return null;

        _ = input ?? throw new ArgumentNullException(nameof(input), "Input cannot be null.");

        var filter = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Call(
                ToLower(propertyExpression.Body),
                ContainsMethod,
                Expression.Constant(ToLower(input), typeof(string))),
            propertyExpression.Parameters);

        return filter;
    }

    /// <inheritdoc />
    public Expression<Func<TEntity, bool>>? StringEquals<TEntity>(
        Expression<Func<TEntity, string>> propertyExpression,
        string? input)
    {
        if (IgnoreDefaults && string.IsNullOrWhiteSpace(input))
            return null;

        var filter = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Equal(
                ToLower(propertyExpression.Body),
                Expression.Constant(ToLower(input), typeof(string))),
            propertyExpression.Parameters);

        return filter;
    }

    /// <inheritdoc />
    public Expression<Func<TEntity, bool>>? Equals<TEntity, TValue>(
        [NotNull] Expression<Func<TEntity, TValue>> propertyExpression,
        TValue input)
    {
        if (IgnoreDefaults && Equals(input, default(TValue)))
            return null;

        if (typeof(TValue) == typeof(string))
            return StringEquals(propertyExpression as Expression<Func<TEntity, string>>, input as string);

        Expression<Func<TEntity, bool>>? filter = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Equal(
                propertyExpression.Body,
                Expression.Constant(input, typeof(TValue))),
            propertyExpression.Parameters);

        return filter;
    }

    /// <inheritdoc />
    public Expression<Func<TEntity, bool>> In<TEntity, TValue>(
        [NotNull] Expression<Func<TEntity, TValue>> propertyExpression,
        [CanBeNull] IEnumerable<TValue> input)
    {
        _ = propertyExpression ??
            throw new ArgumentNullException(nameof(propertyExpression), "Expression cannot be null.");

        if (IgnoreDefaults && input?.Any() != true)
            return null;

        _ = input ?? throw new ArgumentNullException(nameof(input), "Input cannot be null.");

        var inputParameter = Expression.Constant(
            typeof(TValue) == typeof(string)
                ? (IEnumerable<TValue>)((IEnumerable<string?>)input).Select(x => ToLower(x))
                : input);

        Expression<Func<TEntity, bool>> filter = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Call(
                null,
                CollectionContainsMethod.MakeGenericMethod(typeof(TValue)),
                inputParameter,
                typeof(TValue) == typeof(string)
                    ? ToLower(propertyExpression.Body)
                    : propertyExpression.Body),
            propertyExpression.Parameters);

        return filter;
    }

    /// <inheritdoc />
    public Expression<Func<TEntity, bool>> Any<TEntity, TValue>(
        [NotNull] Expression<Func<TEntity, ICollection<TValue>>> collectionSelector,
        [CanBeNull] Expression<Func<TValue, bool>> predicate)
    {
        _ = collectionSelector ??
            throw new ArgumentNullException(nameof(collectionSelector), "Expression cannot be null");

        if (predicate == null)
            return null;

        var anyMethod = AnyMethod.MakeGenericMethod(typeof(TValue));

        Expression<Func<TEntity, bool>> result = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Call(
                null,
                anyMethod,
                collectionSelector.Body,
                predicate),
            collectionSelector.Parameters);

        return result;
    }

    private Expression ToLower(Expression property)
    {
        return IgnoreCase
            ? Expression.Call(property, ToLowerMethod)
            : property;
    }

    private string? ToLower(string? input)
    {
        if (input == null)
            return null;

        if (TrimStrings)
            input = input.Trim();

        return IgnoreCase
            ? input.ToLower()
            : input;
    }
}