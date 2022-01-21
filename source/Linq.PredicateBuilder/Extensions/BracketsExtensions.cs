namespace Linq.PredicateBuilder;

using System;

/// <summary>
/// Contains Brakets extension methods.
/// </summary>
public static class BracketsExtensions
{
    private static Result<TEntity> GetResultInternal<TEntity>(
        IOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
    {
        if (builder == null)
            throw new ArgumentException("Builder cannot be null", nameof(builder));
        
        var init = new BuilderInit<TEntity>(oper.Strategy);
        return new(oper.GetExpression(builder(init).GetExpression()), oper.Strategy);
    }

    /// <summary>
    /// Builds a predicate using nested <paramref name="builder"/>.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="builder">The nested builder.</param>
    public static IAndResult<TEntity> Brackets<TEntity>(
        this IAndOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
        => GetResultInternal(oper, builder);

    /// <summary>
    /// Builds a predicate using nested <paramref name="builder"/>.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="builder">The nested builder.</param>
    public static IOrResult<TEntity> Brackets<TEntity>(
        this IOrOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
        => GetResultInternal(oper, builder);

    /// <summary>
    /// Builds a predicate using nested <paramref name="builder"/>.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="builder">The nested builder.</param>
    public static IFullResult<TEntity> Brackets<TEntity>(
        this IAndOrOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
        => GetResultInternal(oper, builder);
}