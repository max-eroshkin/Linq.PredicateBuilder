namespace Linq.PredicateBuilder;

/// <inheritdoc />
public interface IAndResult<TEntity> : IResult<TEntity>
{
    /// <summary>
    /// Represents logical AND.
    /// </summary>
    IAndOperator<TEntity> And { get; }
}