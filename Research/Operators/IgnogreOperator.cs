namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

public class IgnogreOperator<TEntity> : IAndOrOperator<TEntity>
{
    private readonly IOperator<TEntity> _oper;
    private readonly bool _condition;

    public IgnogreOperator(IOperator<TEntity> oper, bool condition)
    {
        _oper = oper;
        _condition = condition;
    }

    public IOperationStrategy Strategy => _oper.Strategy;

    IAndOrOperator<TEntity> IAndOrOperator<TEntity>.Not => GetNotOperator();

    IAndOperator<TEntity> IAndOperator<TEntity>.Not => GetNotOperator();

    IOrOperator<TEntity> IOrOperator<TEntity>.Not => GetNotOperator();

    private NotOperator<TEntity> GetNotOperator() => new(this, Strategy);

    public Expression<Func<TEntity, bool>>? GetExpression(Expression<Func<TEntity, bool>>? predicate)
    {
        return _oper.GetExpression(_condition ? predicate : null);
    }
}