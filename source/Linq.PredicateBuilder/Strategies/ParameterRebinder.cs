namespace Linq.PredicateBuilder
{
    using System.Linq.Expressions;

    /// <summary>
    /// Parameter rebinder.
    /// </summary>
    internal sealed class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, Expression> _map;

        private ParameterRebinder(Dictionary<ParameterExpression, Expression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, Expression>();
        }

        /// <summary>
        /// Replaces parameter in the expression.
        /// </summary>
        /// <param name="map">Parameter map.</param>
        /// <param name="exp">An expression.</param>
        public static Expression ReplaceParameters(
            Dictionary<ParameterExpression, Expression> map,
            Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        /// <inheritdoc />
        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (_map.TryGetValue(p, out var replacement))
            {
                if (replacement is ParameterExpression pe)
                    p = pe;
                else
                    return replacement;
            }

            return base.VisitParameter(p);
        }
    }
}