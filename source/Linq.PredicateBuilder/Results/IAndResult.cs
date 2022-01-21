namespace Linq.PredicateBuilder;

public interface IAndResult<TEntity> : IResult<TEntity>
{
    IAndOperator<TEntity> And { get; }
}