namespace Linq.PredicateBuilder;

public interface IOrOperator<TEntity> : IOperator<TEntity>
{
    IOrOperator<TEntity> Not { get; }
}