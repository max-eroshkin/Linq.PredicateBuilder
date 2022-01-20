namespace Research;

public interface IAndOrOperator<TEntity> : IAndOperator<TEntity>, IOrOperator<TEntity>
{
    IAndOrOperator<TEntity> Not { get; }
}