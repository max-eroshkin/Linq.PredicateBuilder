namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// Contains In methods.
/// </summary>
public static class InExtensions
{
    /// <summary>
    /// Builds a predicate indicating whether the value defined by a property selector expression
    /// is contained in a specified collection.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector.</param>
    /// <param name="input">The collection to check.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The collection element type.</typeparam>
    public static IAndResult<TEntity> In<TEntity, TInput>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        IEnumerable<TInput>? input)
        => GetResultInternal(oper, propertyExpression, input);

    /// <summary>
    /// Builds a predicate indicating whether the value defined by a property selector expression
    /// is contained in a specified collection.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector.</param>
    /// <param name="input">The collection to check.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The collection element type.</typeparam>
    public static IOrResult<TEntity> In<TEntity, TInput>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        IEnumerable<TInput>? input)
        => GetResultInternal(oper, propertyExpression, input);

    /// <summary>
    /// Builds a predicate indicating whether the value defined by a property selector expression
    /// is contained in a specified collection.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector.</param>
    /// <param name="input">The collection to check.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The collection element type.</typeparam>
    public static IFullResult<TEntity> In<TEntity, TInput>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        IEnumerable<TInput>? input)
        => GetResultInternal(oper, propertyExpression, input);

    private static Result<TEntity> GetResultInternal<TEntity, TValue>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, TValue>> propertyExpression,
        IEnumerable<TValue>? input)
    {
        return new(oper.GetExpression(oper.Strategy.In(propertyExpression, input)), oper.Strategy);
    }
}