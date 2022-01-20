namespace Research;

public interface IAndOperator<TEntity> : IOperator<TEntity>
{
    IAndOperator<TEntity> Not { get; }
}