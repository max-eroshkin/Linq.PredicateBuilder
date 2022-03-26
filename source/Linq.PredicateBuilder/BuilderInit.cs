namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <inheritdoc />
public class BuilderInit<TEntity> : IAndOrOperator<TEntity>
{
    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="strategy">The strategy.</param>
    public BuilderInit(IOperationStrategy? strategy = null)
    {
        Strategy = strategy ?? new OperationStrategy();
    }

    /// <inheritdoc />
    public IOperationStrategy Strategy { get; }

    /// <inheritdoc />
    public IAndOrOperator<TEntity> Not => GetNotOperator();

    /// <inheritdoc />
    IAndOperator<TEntity> IAndOperator<TEntity>.Not => GetNotOperator();

    /// <inheritdoc />
    IOrOperator<TEntity> IOrOperator<TEntity>.Not => GetNotOperator();

    /// <inheritdoc />
    public Expression<Func<TEntity, bool>>? GetExpression(Expression<Func<TEntity, bool>>? predicate)
    {
        return predicate;
    }

    private NotOperator<TEntity> GetNotOperator() => new(this, Strategy);
}