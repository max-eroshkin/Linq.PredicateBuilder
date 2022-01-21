namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// Contains Where methods.
/// </summary>
public static class WhereExtensions
{
    /// <summary>
    /// Uses a specified expression as a predicate.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="predicate">The filtering expression.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IAndResult<TEntity> Where<TEntity>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, bool>> predicate)
        => GetResultInternal(oper, predicate);

    /// <summary>
    /// Uses a specified expression as a predicate.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="predicate">The filtering expression.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IOrResult<TEntity> Where<TEntity>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, bool>> predicate)
        => GetResultInternal(oper, predicate);

    /// <summary>
    /// Uses a specified expression as a predicate.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="predicate">The filtering expression.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IFullResult<TEntity> Where<TEntity>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, bool>> predicate)
        => GetResultInternal(oper, predicate);

    private static Result<TEntity> GetResultInternal<TEntity>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, bool>> filter)
        => new(oper.GetExpression(filter), oper.Strategy);
}