namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

public interface IOperator<TEntity>
{
    public Expression<Func<TEntity, bool>>? GetExpression(Expression<Func<TEntity, bool>>? predicate);
    public IOperationStrategy Strategy { get; }
}