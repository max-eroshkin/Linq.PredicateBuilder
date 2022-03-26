namespace Linq.PredicateBuilder;

/// <inheritdoc cref="IOperator{TEntity}" />
public interface IAndOrOperator<TEntity> : IAndOperator<TEntity>, IOrOperator<TEntity>
{
    /// <summary>
    /// Represents logical NOT.
    /// </summary>
    new IAndOrOperator<TEntity> Not { get; }
}