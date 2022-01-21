namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// Contains 'Contains' extension methods.
/// </summary>
public static class ContainsExtensions
{
    /// <summary>
    /// Builds a predicate indicating whether a specified substring occurs within the string
    /// defined by a property selector expression.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector expression.</param>
    /// <param name="input">The string to seek.</param>
    /// <param name="options">The builder options.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IAndResult<TEntity> Contains<TEntity>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, string>> propertyExpression,
        string? input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);

    /// <summary>
    /// Builds a predicate indicating whether a specified substring occurs within the string
    /// defined by a property selector expression.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector expression.</param>
    /// <param name="input">The string to seek.</param>
    /// <param name="options">The builder options.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IOrResult<TEntity> Contains<TEntity>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, string>> propertyExpression,
        string? input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);

    /// <summary>
    /// Builds a predicate indicating whether a specified substring occurs within the string
    /// defined by a property selector expression.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="propertyExpression">The property selector expression.</param>
    /// <param name="input">The string to seek.</param>
    /// <param name="options">The builder options.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IFullResult<TEntity> Contains<TEntity>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, string>> propertyExpression,
        string? input,
        BuilderOptions? options = null)
        => GetResultInternal(oper, propertyExpression, input, options);

    private static Result<TEntity> GetResultInternal<TEntity>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, string>> propertyExpression,
        string? input,
        BuilderOptions? options = null)
    {
        var strategy = options == null
            ? oper.Strategy
            : new OperationStrategy(options.Value);
        var expression = strategy.Contains(propertyExpression, input);

        return new Result<TEntity>(oper.GetExpression(expression), oper.Strategy);
    }
}