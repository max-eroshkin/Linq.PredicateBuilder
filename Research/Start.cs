namespace Research;

using System.Linq.Expressions;

public class Start<TEntity> : IAndOrOperator<TEntity>
{
    public Start(IOperationStrategy? strategy = null)
    {
        Strategy = strategy ?? new OperationStrategy();
    }

    public IOperationStrategy Strategy { get; }

    public IAndOrOperator<TEntity> Not => GetNotOperator();

    IAndOperator<TEntity> IAndOperator<TEntity>.Not => GetNotOperator();

    IOrOperator<TEntity> IOrOperator<TEntity>.Not => GetNotOperator();

    private NotOperator<TEntity> GetNotOperator() => new(this, Strategy);

    public Expression<Func<TEntity, bool>>? GetExpression(Expression<Func<TEntity, bool>>? predicate)
    {
        return predicate;
    }
}