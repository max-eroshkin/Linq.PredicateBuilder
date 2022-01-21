namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

public static class InExtensions
{
    private static Result<TEntity> GetResultInternal<TEntity, TValue>(
        IOperator<TEntity> oper,
        Expression<Func<TEntity, TValue>> propertyExpression,
        IEnumerable<TValue>? input)
    {
        return new(oper.GetExpression(oper.Strategy.In(propertyExpression, input)), oper.Strategy);
    }

    public static IAndResult<TEntity> In<TEntity, TValue>(
        this IAndOperator<TEntity> oper,
        Expression<Func<TEntity, TValue>> propertyExpression,
        IEnumerable<TValue>? input)
        => GetResultInternal(oper, propertyExpression, input);

    public static IOrResult<TEntity> In<TEntity, TValue>(
        this IOrOperator<TEntity> oper,
        Expression<Func<TEntity, TValue>> propertyExpression,
        IEnumerable<TValue>? input)
        => GetResultInternal(oper, propertyExpression, input);

    public static IFullResult<TEntity> In<TEntity, TValue>(
        this IAndOrOperator<TEntity> oper,
        Expression<Func<TEntity, TValue>> propertyExpression,
        IEnumerable<TValue>? input)
        => GetResultInternal(oper, propertyExpression, input);
}