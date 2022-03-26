namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// The logic operator class.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public interface IOperator<TEntity>
{
    /// <summary>
    /// Gets a builder strategy.
    /// </summary>
    public IOperationStrategy Strategy { get; }

    /// <summary>
    /// Returns the expression combined from the operator's expression and specified predicate expression.
    /// </summary>
    /// <param name="predicate">The filtering expression.</param>
    public Expression<Func<TEntity, bool>>? GetExpression(Expression<Func<TEntity, bool>>? predicate);
}