namespace Linq.PredicateBuilder;

using System.Linq.Expressions;

/// <summary>
/// The operator for condotional building.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class IgnogreOperator<TEntity> : IAndOrOperator<TEntity>
{
    private readonly IOperator<TEntity> _oper;
    private readonly bool _condition;

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="oper">The operator instance.</param>
    /// <param name="condition">The condition to evaluate.</param>
    public IgnogreOperator(IOperator<TEntity> oper, bool condition)
    {
        _oper = oper;
        _condition = condition;
    }

    /// <inheritdoc />
    public IOperationStrategy Strategy => _oper.Strategy;

    IAndOrOperator<TEntity> IAndOrOperator<TEntity>.Not => GetNotOperator();

    IAndOperator<TEntity> IAndOperator<TEntity>.Not => GetNotOperator();

    IOrOperator<TEntity> IOrOperator<TEntity>.Not => GetNotOperator();

    private NotOperator<TEntity> GetNotOperator() => new(this, Strategy);

    /// <inheritdoc />
    public Expression<Func<TEntity, bool>>? GetExpression(Expression<Func<TEntity, bool>>? predicate)
    {
        return _oper.GetExpression(_condition ? predicate : null);
    }
}