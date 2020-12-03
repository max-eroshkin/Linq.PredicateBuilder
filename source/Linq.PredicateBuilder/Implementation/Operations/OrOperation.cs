namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Операция логического "Или"
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    internal class OrOperation<TEntity> : LogicOperation<TEntity>, IOrLogicOperation<TEntity>
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

        /// <inheritdoc/>
        IOrLogicOperation<TEntity> IOrLogicOperation<TEntity>.Not => new NotOrOperation<TEntity>(Operation, Strategy);

        /// <inheritdoc />
        protected override Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> Operation =>
            x => BuilderResult.GetExpression().Or(x);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Contains(
            Expression<Func<TEntity, string>> propertyExpression,
            string input) => ContainsInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Equals<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            TValue input)
            => EqualsInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate)
            => WhereInternal(predicate);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> In<TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression,
            IEnumerable<TValue> input)
            => InInternal(propertyExpression, input);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Any<TValue>(
            Expression<Func<TEntity, ICollection<TValue>>> manyToManySelector,
            Func<ILogicOperation<TValue>, IQueryBuilderResult<TValue>> builder)
            => AnyInternal(manyToManySelector, builder);

        /// <inheritdoc/>
        public IOrQueryBuilderResult<TEntity> Brackets(
            Func<ILogicOperation<TEntity>, IQueryBuilderResult<TEntity>> builder)
            => BracketsInternal(builder);

        /// <inheritdoc />
        public IOrLogicOperation<TEntity> Conditional(bool condition)
            => new ConditionOrOperation<TEntity>(Operation, condition, Strategy);
    }
}