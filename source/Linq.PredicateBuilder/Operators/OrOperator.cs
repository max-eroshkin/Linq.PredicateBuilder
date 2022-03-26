namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <inheritdoc />
public class OrOperator<TEntity> : IOrOperator<TEntity>
{
    private readonly Expression<Func<TEntity, bool>>? _predicate;

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="predicate">The predicate expression.</param>
    /// <param name="strategy">The builder strategy.</param>
    public OrOperator(Expression<Func<TEntity, bool>>? predicate, IOperationStrategy strategy)
    {
        Strategy = strategy;
        _predicate = predicate;
    }

    /// <inheritdoc />
    public IOrOperator<TEntity> Not => new NotOperator<TEntity>(this, Strategy);

    /// <inheritdoc />
    public IOperationStrategy Strategy { get; }

    /// <inheritdoc />
    public Expression<Func<TEntity, bool>>? GetExpression(Expression<Func<TEntity, bool>>? predicate)
    {
        return _predicate.Or(predicate);
    }
}