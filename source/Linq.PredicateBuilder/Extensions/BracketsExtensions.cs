namespace Linq.PredicateBuilder;

/// <summary>
/// Contains Brakets extension methods.
/// </summary>
public static class BracketsExtensions
{
    /// <summary>
    /// Builds a predicate using nested <paramref name="builder"/>.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="builder">The nested builder.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IAndResult<TEntity> Brackets<TEntity>(
        this IAndOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
        => GetResultInternal(oper, builder);

    /// <summary>
    /// Builds a predicate using nested <paramref name="builder"/>.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="builder">The nested builder.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IOrResult<TEntity> Brackets<TEntity>(
        this IOrOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
        => GetResultInternal(oper, builder);

    /// <summary>
    /// Builds a predicate using nested <paramref name="builder"/>.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="builder">The nested builder.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IFullResult<TEntity> Brackets<TEntity>(
        this IAndOrOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
        => GetResultInternal(oper, builder);

    private static Result<TEntity> GetResultInternal<TEntity>(
        IOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
    {
        if (builder == null)
            throw new ArgumentException("Builder cannot be null", nameof(builder));

        var init = new BuilderInit<TEntity>(oper.Strategy);
        return new(oper.GetExpression(builder(init).GetExpression()), oper.Strategy);
    }
}