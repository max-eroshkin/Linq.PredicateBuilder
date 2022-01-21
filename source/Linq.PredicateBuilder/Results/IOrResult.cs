namespace Linq.PredicateBuilder;

/// <inheritdoc />
public interface IOrResult<TEntity> : IResult<TEntity>
{
    /// <summary>
    /// Represents logical OR.
    /// </summary>
    IOrOperator<TEntity> Or { get; }
}