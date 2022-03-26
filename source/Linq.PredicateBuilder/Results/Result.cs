namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <inheritdoc />
public class Result<TEntity> : IFullResult<TEntity>
{
    private readonly Expression<Func<TEntity, bool>>? _predicate;
    private readonly IOperationStrategy _strategy;

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="predicate">The filtering expression.</param>
    /// <param name="strategy">The builder strategy.</param>
    public Result(Expression<Func<TEntity, bool>>? predicate, IOperationStrategy strategy)
    {
        _predicate = predicate;
        _strategy = strategy;
    }

    /// <inheritdoc />
    public IAndOperator<TEntity> And => new AndOperator<TEntity>(GetExpression(), _strategy);

    /// <inheritdoc />
    public IOrOperator<TEntity> Or => new OrOperator<TEntity>(GetExpression(), _strategy);

    /// <inheritdoc />
    public Expression<Func<TEntity, bool>>? GetExpression()
    {
        return _predicate;
    }
}