namespace Linq.PredicateBuilder;

public interface IAndOrOperator<TEntity> : IAndOperator<TEntity>, IOrOperator<TEntity>
{
    new IAndOrOperator<TEntity> Not { get; }
}