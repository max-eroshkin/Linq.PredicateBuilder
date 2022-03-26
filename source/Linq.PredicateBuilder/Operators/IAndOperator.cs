namespace Linq.PredicateBuilder;

/// <inheritdoc />
public interface IAndOperator<TEntity> : IOperator<TEntity>
{
    /// <summary>
    /// Represents logical NOT.
    /// </summary>
    IAndOperator<TEntity> Not { get; }
}