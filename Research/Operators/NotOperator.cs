namespace Research;

using System.Linq.Expressions;

public class NotOperator<TEntity> : IAndOrOperator<TEntity>
{
    private readonly IOperator<TEntity> _oper;

    public NotOperator(IOperator<TEntity> oper, IOperationStrategy strategy)
    {
        _oper = oper;
        Strategy = strategy;
    }
    public Expression<Func<TEntity, bool>>? GetExpression(Expression<Func<TEntity, bool>>? predicate)
    {
        return _oper.GetExpression(predicate.Not());
    }

    public IOperationStrategy Strategy { get; }

    IAndOrOperator<TEntity> IAndOrOperator<TEntity>.Not => GetNotOperator();

    IAndOperator<TEntity> IAndOperator<TEntity>.Not => GetNotOperator();

    IOrOperator<TEntity> IOrOperator<TEntity>.Not => GetNotOperator();

    private NotOperator<TEntity> GetNotOperator() => new(this, Strategy);
}