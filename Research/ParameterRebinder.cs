namespace Linq.PredicateBuilder;

using System.Collections.Generic;
using System.Linq.Expressions;

/// <summary>
/// Parameter rebinder.
/// </summary>
internal sealed class ParameterRebinder : ExpressionVisitor
{
    private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

    private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
    {
        _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    /// <summary>
    /// Replaces parameter in the expression.
    /// </summary>
    /// <param name="map">Parameter map.</param>
    /// <param name="exp">An expression.</param>
    public static Expression ReplaceParameters(
        Dictionary<ParameterExpression, ParameterExpression> map,
        Expression exp)
    {
        return new ParameterRebinder(map).Visit(exp);
    }

    /// <inheritdoc />
    protected override Expression VisitParameter(ParameterExpression p)
    {
        if (_map.TryGetValue(p, out var replacement))
        {
            p = replacement;
        }

        return base.VisitParameter(p);
    }
}