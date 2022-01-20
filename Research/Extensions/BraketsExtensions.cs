namespace Linq.PredicateBuilder;

public static class BraketsExtensions
{
    private static Result<TEntity> GetResultInternal<TEntity>(
        IOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
    {
        if (builder == null)
            throw new ArgumentException("Builder cannot be null", nameof(builder));
        
        var init = new Start<TEntity>(oper.Strategy);
        return new(oper.GetExpression(builder(init).GetExpression()), oper.Strategy);
    }

    public static IAndResult<TEntity> Brackets<TEntity>(
        this IAndOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
        => GetResultInternal(oper, builder);

    public static IOrResult<TEntity> Brackets<TEntity>(
        this IOrOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
        => GetResultInternal(oper, builder);

    public static IFullResult<TEntity> Brackets<TEntity>(
        this IAndOrOperator<TEntity> oper,
        Func<IAndOrOperator<TEntity>, IResult<TEntity>> builder)
        => GetResultInternal(oper, builder);
}