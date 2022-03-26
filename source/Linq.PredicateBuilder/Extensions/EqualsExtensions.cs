namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// Contains Equals methods.
/// </summary>
public static class EqualsExtensions
{
    /// <summary>
    /// Builds a predicate indicating whether a specified input value is equal to the value
    /// defined by a property selector expression.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector.</param>
    /// <param name="input">The value to compare with.</param>
    /// <param name="options">The builder options.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The property and input value type.</typeparam>
    public static IAndResult<TEntity> Equals<TEntity, TInput>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        TInput input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);

    /// <summary>
    /// Builds a predicate indicating whether a specified input value is equal to the value
    /// defined by a property selector expression.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector.</param>
    /// <param name="input">The value to compare with.</param>
    /// <param name="options">The builder options.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The property and input value type.</typeparam>
    public static IOrResult<TEntity> Equals<TEntity, TInput>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        TInput input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);

    /// <summary>
    /// Builds a predicate indicating whether a specified input value is equal to the value
    /// defined by a property selector expression.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector.</param>
    /// <param name="input">The value to compare with.</param>
    /// <param name="options">The builder options.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInput">The property and input value type.</typeparam>
    public static IFullResult<TEntity> Equals<TEntity, TInput>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        TInput input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);

    private static Result<TEntity> GetResultInternal<TEntity, TInput>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, TInput>> propertyExpression,
        TInput input,
        BuilderOptions? options = null)
    {
        var strategy = options == null
            ? oper.Strategy
            : new OperationStrategy(options.Value);
        var expression = strategy.Equals(propertyExpression, input);

        return new Result<TEntity>(oper.GetExpression(expression), oper.Strategy);
    }
}