namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// Represents a builder result.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public interface IResult<TEntity>
{
    /// <summary>
    /// Возвращает выражение предиката, построенное билдером
    /// </summary>
    Expression<Func<TEntity, bool>>? GetExpression();
}