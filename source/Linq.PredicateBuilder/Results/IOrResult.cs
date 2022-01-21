namespace Linq.PredicateBuilder;

public interface IOrResult<TEntity> : IResult<TEntity>
{
    IOrOperator<TEntity> Or { get; }
}