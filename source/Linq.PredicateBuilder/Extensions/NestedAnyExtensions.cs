namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// Contains nested Any methods.
/// </summary>
public static class NestedAnyExtensions
{
    /// <summary>
    /// Builds a predicate based on filter for the nested collection property
    /// defined by a property selector expression.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="collectionSelector">The nested collection property selector.</param>
    /// <param name="builder">The nested filter builder.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The nested collection element type.</typeparam>
    public static IAndResult<TEntity> Any<TEntity, TInput>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, IEnumerable<TInput>>> collectionSelector,
        Func<IAndOrOperator<TInput>, IResult<TInput>> builder)
        => GetResultInternal(oper, collectionSelector, builder);

    /// <summary>
    /// Builds a predicate based on filter for the nested collection property
    /// defined by a property selector expression.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="collectionSelector">The nested collection property selector.</param>
    /// <param name="builder">The nested filter builder.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The nested collection element type.</typeparam>
    public static IOrResult<TEntity> Any<TEntity, TInput>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, IEnumerable<TInput>>> collectionSelector,
        Func<IAndOrOperator<TInput>, IResult<TInput>> builder)
        => GetResultInternal(oper, collectionSelector, builder);

    /// <summary>
    /// Builds a predicate based on filter for the nested collection property
    /// defined by a property selector expression.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="collectionSelector">The nested collection property selector.</param>
    /// <param name="builder">The nested filter builder.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The nested collection element type.</typeparam>
    public static IFullResult<TEntity> Any<TEntity, TInput>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, IEnumerable<TInput>>> collectionSelector,
        Func<IAndOrOperator<TInput>, IResult<TInput>> builder)
        => GetResultInternal(oper, collectionSelector, builder);

    private static Result<TEntity> GetResultInternal<TEntity, TInput>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, IEnumerable<TInput>>> collectionSelector,
        Func<IAndOrOperator<TInput>, IResult<TInput>> builder)
    {
        if (builder == null)
            throw new ArgumentException("Builder cannot be null", nameof(builder));

        var init = new BuilderInit<TInput>(oper.Strategy);
        return new(
            oper.GetExpression(oper.Strategy.Any(collectionSelector, builder(init).GetExpression())),
            oper.Strategy);
    }
}