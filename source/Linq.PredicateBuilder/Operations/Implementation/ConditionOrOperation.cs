namespace Linq.PredicateBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Операция отрицания
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности в выражении</typeparam>
    public class ConditionOrOperation<TEntity> : ConditionOperationBase<TEntity>, IOrLogicOperation<TEntity>
    {
        /// <inheritdoc />
        public ConditionOrOperation(
            Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>> baseOperation,
            bool condition,
            IOperationStrategy strategy)
            : base(baseOperation, condition, strategy)
        {
        }

        /// <inheritdoc />
        public IOrLogicOperation<TEntity> Not => new NotOrOperation<TEntity>(Operation, Strategy);

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
            Func<QueryBuilder<TValue>, IQueryBuilderResult<TValue>> builder)
            => AnyInternal(manyToManySelector, builder);

        /// <inheritdoc />
        public IOrQueryBuilderResult<TEntity> Brackets(
            Func<QueryBuilder<TEntity>, IQueryBuilderResult<TEntity>> builder)
            => BracketsInternal(builder);

        /// <inheritdoc/>
        public IOrLogicOperation<TEntity> Conditional(bool condition)
            => new ConditionOrOperation<TEntity>(Operation, condition, Strategy);

        /// <inheritdoc />
        public IOrQueryBuilderResult<TEntity> Contains(
            Expression<Func<TEntity, string>> propertyExpression,
            string input)
            => ContainsInternal(propertyExpression, input);
    }
}