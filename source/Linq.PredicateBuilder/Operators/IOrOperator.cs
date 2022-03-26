namespace Linq.PredicateBuilder;

/// <inheritdoc />
public interface IOrOperator<TEntity> : IOperator<TEntity>
{
    /// <summary>
    /// Represents logical NOT.
    /// </summary>
    IOrOperator<TEntity> Not { get; }
}