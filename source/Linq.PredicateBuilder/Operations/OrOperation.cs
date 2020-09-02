namespace Linq.PredicateBuilder
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Операция логического "Или"
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public class OrOperation<TEntity> : LogicOperation<TEntity>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="builderResult">Промежуточный билдер</param>
        /// <param name="strategy">A filtering strategy.</param>
        public OrOperation(QueryBuilderResult<TEntity> builderResult, IOperationStrategy strategy)
            : base(builderResult, strategy)
        {
        }

        /// <inheritdoc />
        protected override Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation =>
            x => BuilderResult.GetExpression().Or(x);
    }
}