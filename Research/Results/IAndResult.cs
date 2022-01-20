namespace Research;

public interface IAndResult<TEntity> : IResult<TEntity>
{
    IAndOperator<TEntity> And { get; }
}