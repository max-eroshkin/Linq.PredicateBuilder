namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// Contains conditional Where methods.
/// </summary>
public static class ConditionalWhere
{
    /// <summary>
    /// Uses a specified expression as a predicate and a specified input value as its parameter.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="predicate">The filtering expression.</param>
    /// <param name="input">The value to use as a parameter.</param>
    /// <param name="options">The builder options.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The input value type.</typeparam>
    public static IAndResult<TEntity> Where<TEntity, TInput>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, TInput, bool>> predicate,
        TInput? input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, predicate, input, options);

    /// <summary>
    /// Uses a specified expression as a predicate and a specified input value as its parameter.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="predicate">The filtering expression.</param>
    /// <param name="input">The value to use as a parameter.</param>
    /// <param name="options">The builder options.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The input value type.</typeparam>
    public static IOrResult<TEntity> Where<TEntity, TInput>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, TInput, bool>> predicate,
        TInput? input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, predicate, input, options);

    /// <summary>
    /// Uses a specified expression as a predicate and a specified input value as its parameter.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="predicate">The filtering expression.</param>
    /// <param name="input">The value to use as a parameter.</param>
    /// <param name="options">The builder options.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The input value type.</typeparam>
    public static IFullResult<TEntity> Where<TEntity, TInput>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, TInput, bool>> predicate,
        TInput? input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, predicate, input, options);

    private static Result<TEntity> GetResultInternal<TEntity, TInput>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, TInput, bool>> predicate,
        TInput? input,
        BuilderOptions? options = null)
    {
        var strategy = options == null
            ? oper.Strategy
            : new OperationStrategy(options.Value);
        var expression = strategy.Where(predicate, input);
        return new(oper.GetExpression(expression), oper.Strategy);
    }
}