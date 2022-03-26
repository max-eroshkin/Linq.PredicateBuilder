namespace Linq.PredicateBuilder;

/// <summary>
/// Contains conditional evaluation methods.
/// </summary>
public static class ConditionalExtensions
{
    /// <summary>
    /// Evaluates the first trailing builder expression if only the specified condition evaluates to true.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IAndOperator<TEntity> Conditional<TEntity>(this IAndOperator<TEntity> oper, bool condition)
        => GetOperatorInternal(oper, condition);

    /// <summary>
    /// Evaluates the first trailing builder expression if only the specified condition evaluates to true.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IOrOperator<TEntity> Conditional<TEntity>(this IOrOperator<TEntity> oper, bool condition)
        => GetOperatorInternal(oper, condition);

    /// <summary>
    /// Evaluates the first trailing builder expression if only the specified condition evaluates to true.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IAndOrOperator<TEntity> Conditional<TEntity>(this IAndOrOperator<TEntity> oper, bool condition)
        => GetOperatorInternal(oper, condition);

    private static IgnogreOperator<TEntity> GetOperatorInternal<TEntity>(IOperator<TEntity> oper, bool condition)
    {
        return new IgnogreOperator<TEntity>(oper, condition);
    }
}