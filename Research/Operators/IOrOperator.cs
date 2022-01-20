namespace Research;

public interface IOrOperator<TEntity> : IOperator<TEntity>
{
    IOrOperator<TEntity> Not { get; }
}