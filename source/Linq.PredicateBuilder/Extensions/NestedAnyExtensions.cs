namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

public static class NestedAnyExtensions
{
    private static Result<TEntity> GetResultInternal<TEntity, TInput>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, ICollection<TInput>>> collectionSelector,
        Func<IAndOrOperator<TInput>, IResult<TInput>> builder)
    {
        if (builder == null)
            throw new ArgumentException("Builder cannot be null", nameof(builder));

        var init = new BuilderInit<TInput>(oper.Strategy);
        return new(
            oper.GetExpression(oper.Strategy.Any(collectionSelector, builder(init).GetExpression())),
            oper.Strategy);
    }

    public static IAndResult<TEntity> Any<TEntity, TInput>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, ICollection<TInput>>> collectionSelector,
        Func<IAndOrOperator<TInput>, IResult<TInput>> builder)
        => GetResultInternal(oper, collectionSelector, builder);

    public static IOrResult<TEntity> Any<TEntity, TInput>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, ICollection<TInput>>> collectionSelector,
        Func<IAndOrOperator<TInput>, IResult<TInput>> builder)
        => GetResultInternal(oper, collectionSelector, builder);

    public static IFullResult<TEntity> Any<TEntity, TInput>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, ICollection<TInput>>> collectionSelector,
        Func<IAndOrOperator<TInput>, IResult<TInput>> builder)
        => GetResultInternal(oper, collectionSelector, builder);
}