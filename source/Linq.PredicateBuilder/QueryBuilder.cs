namespace Linq.PredicateBuilder
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Билдер выражения поиска
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    public class QueryBuilder<TEntity> : LogicOperation<TEntity>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="strategy">A filtering strategy.</param>
        public QueryBuilder(IOperationStrategy strategy)
            : base(new QueryBuilderResult<TEntity>(strategy), strategy)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation => x => x;
    }
}