namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

public interface IResult<TEntity>
{
    /// <summary>
    /// Возвращает выражение предиката, построенное билдером
    /// </summary>
    Expression<Func<TEntity, bool>>? GetExpression();
}