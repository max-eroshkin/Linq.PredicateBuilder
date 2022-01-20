namespace Research;

using System.Linq.Expressions;

public class Result<TEntity> : IFullResult<TEntity>
{
    private readonly Expression<Func<TEntity, bool>>? _predicate;
    private readonly IOperationStrategy _strategy;

    public Result(Expression<Func<TEntity, bool>>? predicate, IOperationStrategy strategy)
    {
        _predicate = predicate;
        _strategy = strategy;
    }
    
    public Expression<Func<TEntity, bool>>? GetExpression()
    {
        return _predicate;
    }

    public IAndOperator<TEntity> And => new AndOperator<TEntity>(GetExpression(), _strategy);

    public IOrOperator<TEntity> Or { get; }
}