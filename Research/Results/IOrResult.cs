namespace Research;

public interface IOrResult<TEntity> : IResult<TEntity>
{
    IOrOperator<TEntity> Or { get; }
}