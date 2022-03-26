namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// Represents logical NOT.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public class NotOperator<TEntity> : IAndOrOperator<TEntity>
{
    private readonly IOperator<TEntity> _oper;

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="strategy">The builder strategy.</param>
    public NotOperator(IOperator<TEntity> oper, IOperationStrategy strategy)
    {
        _oper = oper;
        Strategy = strategy;
    }

    /// <inheritdoc />
    public IOperationStrategy Strategy { get; }

    /// <summary>
    /// Represents logical NOT.
    /// </summary>
    IAndOrOperator<TEntity> IAndOrOperator<TEntity>.Not => GetNotOperator();

    /// <summary>
    /// Represents logical NOT.
    /// </summary>
    IAndOperator<TEntity> IAndOperator<TEntity>.Not => GetNotOperator();

    /// <summary>
    /// Represents logical NOT.
    /// </summary>
    IOrOperator<TEntity> IOrOperator<TEntity>.Not => GetNotOperator();

    /// <inheritdoc />
    public Expression<Func<TEntity, bool>>? GetExpression(Expression<Func<TEntity, bool>>? predicate)
    {
        return _oper.GetExpression(predicate.Not());
    }

    private NotOperator<TEntity> GetNotOperator() => new(this, Strategy);
}