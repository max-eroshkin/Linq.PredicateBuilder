namespace Linq.PredicateBuilder
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// An intermediate query builder.
    /// </summary>
    /// <typeparam name="TEntity">The data source type.</typeparam>
    public interface IQueryBuilderResult<TEntity>
    {
        /// <summary>
        /// Returns a predicate based on the builder.
        /// </summary>
        Expression<Func<TEntity, bool>> GetExpression();
    }
}