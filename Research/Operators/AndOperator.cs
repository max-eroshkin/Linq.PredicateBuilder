namespace Research;

using System.Linq.Expressions;

public class AndOperator<TEntity> : IAndOperator<TEntity>
{
    private readonly Expression<Func<TEntity, bool>> _predicate;

    public AndOperator(Expression<Func<TEntity, bool>> predicate, IOperationStrategy strategy)
    {
        Strategy = strategy;
        _predicate = predicate;
    }

    public IAndOperator<TEntity> Not => new NotOperator<TEntity>(this, Strategy);

    public Expression<Func<TEntity, bool>> GetExpression(Expression<Func<TEntity, bool>> predicate)
    {
        return _predicate.And(predicate); 
    }

    public IOperationStrategy Strategy { get; }
}