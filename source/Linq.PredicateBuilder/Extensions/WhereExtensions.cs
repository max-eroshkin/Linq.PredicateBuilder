namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

public static class WhereExtensions
{
    private static Result<TEntity> GetResultInternal<TEntity>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, bool>> filter) 
        => new(oper.GetExpression(filter), oper.Strategy);

    public static IAndResult<TEntity> Where<TEntity>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, bool>> filter)
        => GetResultInternal(oper, filter);

    public static IOrResult<TEntity> Where<TEntity>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, bool>> filter)
        => GetResultInternal(oper, filter);

    public static IFullResult<TEntity> Where<TEntity>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, bool>> filter)
        => GetResultInternal(oper, filter);
}